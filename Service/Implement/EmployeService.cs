using GRH_SENTECH.Data;
using GRH_SENTECH.Entity;
using GRH_SENTECH.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GRH_SENTECH.Service.Implement
{

    public class EmployeService : IEmployeService
    {
        private readonly IEmployeRepository _employeRepository;
        private readonly IContratRepository _contratRepository;
        private readonly IDepartementRepository _departementRepository;
        private readonly IPosteRepository _posteRepository;
        private readonly ICongeRepository _congeRepository;
        private readonly GrhDbContext _context;
        private readonly ILogger<EmployeService> _logger;

        public EmployeService(
            IEmployeRepository employeRepository,
            IContratRepository contratRepository,
            IDepartementRepository departementRepository,
            IPosteRepository posteRepository,
            ICongeRepository congeRepository,
            GrhDbContext context,
            ILogger<EmployeService> logger)
        {
            _employeRepository = employeRepository;
            _contratRepository = contratRepository;
            _departementRepository = departementRepository;
            _posteRepository = posteRepository;
            _congeRepository = congeRepository;
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Employe> GetAllEmployes(int offset = 0, int limit = 10)
        {
            try
            {
                return _employeRepository.FindAll(offset, limit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des employés");
                return new List<Employe>();
            }
        }

        public Employe? GetEmployeById(int id)
        {
            try
            {
                return _employeRepository.FindById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'employé {Id}", id);
                return null;
            }
        }

        public ServiceResult<Employe> CreateEmploye(Employe employe)
        {
            try
            {
                var errors = new List<string>();

                var age = (employe.DateEmbauche - employe.DateNaissance).Days / 365;
                if (age < 18)
                {
                    errors.Add("RG05 : L'employé doit avoir au moins 18 ans à la date d'embauche.");
                }

                var existingMatricule = _employeRepository.FindByMatricule(employe.Matricule);
                if (existingMatricule != null)
                {
                    errors.Add($"Un employé avec le matricule '{employe.Matricule}' existe déjà.");
                }

                var departement = _departementRepository.FindById(employe.DepartementId);
                if (departement == null)
                {
                    errors.Add("Le département sélectionné n'existe pas.");
                }

                if (errors.Any())
                    return ServiceResult<Employe>.Fail(errors);

                bool result = _employeRepository.Add(employe);
                if (result)
                    return ServiceResult<Employe>.Ok(employe, "Employé créé avec succès !");

                return ServiceResult<Employe>.Fail("Erreur lors de la création de l'employé.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'employé");
                return ServiceResult<Employe>.Fail("Une erreur inattendue est survenue.");
            }
        }

        public ServiceResult<Employe> UpdateEmploye(Employe employe)
        {
            try
            {
                var existing = _employeRepository.FindById(employe.Id);
                if (existing == null)
                    return ServiceResult<Employe>.Fail("Employé introuvable.");

                // RG05 : Vérification de l'âge
                var age = (employe.DateEmbauche - employe.DateNaissance).Days / 365;
                if (age < 18)
                    return ServiceResult<Employe>.Fail("RG05 : L'employé doit avoir au moins 18 ans à la date d'embauche.");

                bool result = _employeRepository.Update(employe);
                if (result)
                    return ServiceResult<Employe>.Ok(employe, "Employé modifié avec succès !");

                return ServiceResult<Employe>.Fail("Erreur lors de la modification.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la modification de l'employé");
                return ServiceResult<Employe>.Fail("Une erreur inattendue est survenue.");
            }
        }

        public ServiceResult<bool> DeleteEmploye(int id)
        {
            try
            {
                var employe = _employeRepository.FindById(id);
                if (employe == null)
                    return ServiceResult<bool>.Fail("Employé introuvable.");

                var contratActif = _contratRepository.FindContratActif(id);
                if (contratActif != null && contratActif.TypeContrat == TypeContrat.CDI)
                    return ServiceResult<bool>.Fail("RG03 : Impossible de supprimer un employé avec un contrat CDI actif.");

                var congesEnAttente = _context.Conges
                    .Any(c => c.EmployeId == id && c.Statut == StatutConge.EnAttente);
                if (congesEnAttente)
                    return ServiceResult<bool>.Fail("RG03 : Impossible de supprimer un employé avec des congés en attente.");

                bool result = _employeRepository.Delete(id);
                if (result)
                    return ServiceResult<bool>.Ok(true, "Employé supprimé avec succès !");

                return ServiceResult<bool>.Fail("Erreur lors de la suppression.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'employé {Id}", id);
                return ServiceResult<bool>.Fail("Une erreur inattendue est survenue.");
            }
        }

        public ServiceResult<Contrat> AjouterContrat(Contrat contrat)
        {
            try
            {
                var errors = new List<string>();

                var contratActif = _contratRepository.FindContratActif(contrat.EmployeId);
                if (contratActif != null)
                    errors.Add("RG01 : L'employé a déjà un contrat actif. Clôturez-le avant d'en créer un nouveau.");

                var poste = _posteRepository.FindById(contrat.PosteId);
                if (poste != null)
                {
                    if (contrat.SalaireBase < poste.SalaireMin || contrat.SalaireBase > poste.SalaireMax)
                        errors.Add($"RG02 : Le salaire doit être entre {poste.SalaireMin:N0} et {poste.SalaireMax:N0} FCFA pour ce poste.");
                }

                var employe = _employeRepository.FindById(contrat.EmployeId);
                if (employe != null)
                {
                    var departement = _departementRepository.FindById(employe.DepartementId);
                    if (departement != null)
                    {
                        var totalSalaires = _context.Contrats
                            .Where(c => c.Employe!.DepartementId == employe.DepartementId && c.IsActif)
                            .Sum(c => c.SalaireBase);

                        if (totalSalaires + contrat.SalaireBase > departement.Budget)
                            errors.Add($"RG07 : Ce contrat dépasse le budget du département ({departement.Budget:N0} FCFA).");
                    }
                }

                if (errors.Any())
                    return ServiceResult<Contrat>.Fail(errors);

                bool result = _contratRepository.Add(contrat);
                if (result)
                    return ServiceResult<Contrat>.Ok(contrat, "Contrat ajouté avec succès !");

                return ServiceResult<Contrat>.Fail("Erreur lors de l'ajout du contrat.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajout du contrat");
                return ServiceResult<Contrat>.Fail("Une erreur inattendue est survenue.");
            }
        }

        public ServiceResult<bool> TransfertEmploye(int employeId, int nouveauDepartementId, int nouveauPosteId, decimal nouveauSalaire)
        {

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var employe = _employeRepository.FindById(employeId);
                if (employe == null)
                    return ServiceResult<bool>.Fail("Employé introuvable.");

                var nouveauDept = _departementRepository.FindById(nouveauDepartementId);
                if (nouveauDept == null)
                    return ServiceResult<bool>.Fail("Département de destination introuvable.");

                var poste = _posteRepository.FindById(nouveauPosteId);
                if (poste == null)
                    return ServiceResult<bool>.Fail("Poste introuvable.");

                if (nouveauSalaire < poste.SalaireMin || nouveauSalaire > poste.SalaireMax)
                    return ServiceResult<bool>.Fail($"RG02 : Le salaire doit être entre {poste.SalaireMin:N0} et {poste.SalaireMax:N0} FCFA.");

                var totalSalairesNouveauDept = _context.Contrats
                    .Where(c => c.Employe!.DepartementId == nouveauDepartementId && c.IsActif)
                    .Sum(c => c.SalaireBase);

                if (totalSalairesNouveauDept + nouveauSalaire > nouveauDept.Budget)
                    return ServiceResult<bool>.Fail("RG07 : Ce transfert dépasse le budget du département de destination.");

                var ancienContrat = _contratRepository.FindContratActif(employeId);
                if (ancienContrat != null)
                    _contratRepository.ClotureContrat(ancienContrat.Id);

                employe.DepartementId = nouveauDepartementId;
                _employeRepository.Update(employe);

                var nouveauContrat = new Contrat
                {
                    EmployeId = employeId,
                    PosteId = nouveauPosteId,
                    TypeContrat = TypeContrat.CDI,
                    DateDebut = DateTime.Now,
                    SalaireBase = nouveauSalaire,
                    IsActif = true
                };
                _contratRepository.Add(nouveauContrat);

                transaction.Commit();
                return ServiceResult<bool>.Ok(true, $"Employé transféré avec succès vers {nouveauDept.Nom}.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Erreur lors du transfert de l'employé {Id}", employeId);
                return ServiceResult<bool>.Fail("Erreur lors du transfert. Transaction annulée.");
            }
        }

        public int CountEmployes()
        {
            return _employeRepository.Count();
        }

        public IEnumerable<Employe> GetEmployesByDepartement(int departementId)
        {
            return _employeRepository.FindByDepartement(departementId);
        }
    }
}
