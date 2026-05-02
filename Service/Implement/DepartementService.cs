using GRH_SENTECH.Entity;
using GRH_SENTECH.Repository;
using Microsoft.Extensions.Logging;

namespace GRH_SENTECH.Service.Implement
{
    public class DepartementService : IDepartementService
    {
        private readonly IDepartementRepository _departementRepository;
        private readonly ILogger<DepartementService> _logger;

        public DepartementService(IDepartementRepository departementRepository, ILogger<DepartementService> logger)
        {
            _departementRepository = departementRepository;
            _logger = logger;
        }

        public IEnumerable<Departement> GetAllDepartements()
        {
            try
            {
                return _departementRepository.FindAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des départements");
                return new List<Departement>();
            }
        }

        public Departement? GetDepartementById(int id)
        {
            try
            {
                return _departementRepository.FindById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du département {Id}", id);
                return null;
            }
        }

        public ServiceResult<Departement> CreateDepartement(Departement departement)
        {
            try
            {
                // Vérification unicité du code
                var existing = _departementRepository.FindAll()
                    .Any(d => d.Code.ToUpper() == departement.Code.ToUpper());
                if (existing)
                    return ServiceResult<Departement>.Fail($"Un département avec le code '{departement.Code}' existe déjà.");

                departement.Code = departement.Code.ToUpper();
                bool result = _departementRepository.Add(departement);
                if (result)
                    return ServiceResult<Departement>.Ok(departement, "Département créé avec succès !");

                return ServiceResult<Departement>.Fail("Erreur lors de la création.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du département");
                return ServiceResult<Departement>.Fail("Une erreur inattendue est survenue.");
            }
        }

        public ServiceResult<Departement> UpdateDepartement(Departement departement)
        {
            try
            {
                var existing = _departementRepository.FindById(departement.Id);
                if (existing == null)
                    return ServiceResult<Departement>.Fail("Département introuvable.");

                departement.Code = departement.Code.ToUpper();
                bool result = _departementRepository.Update(departement);
                if (result)
                    return ServiceResult<Departement>.Ok(departement, "Département modifié avec succès !");

                return ServiceResult<Departement>.Fail("Erreur lors de la modification.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la modification du département");
                return ServiceResult<Departement>.Fail("Une erreur inattendue est survenue.");
            }
        }

        public ServiceResult<bool> DeleteDepartement(int id)
        {
            try
            {
                var dept = _departementRepository.FindById(id);
                if (dept == null)
                    return ServiceResult<bool>.Fail("Département introuvable.");

                if (dept.Employes != null && dept.Employes.Any(e => e.IsActif))
                    return ServiceResult<bool>.Fail("Impossible de supprimer un département avec des employés actifs.");

                bool result = _departementRepository.Delete(id);
                if (result)
                    return ServiceResult<bool>.Ok(true, "Département supprimé avec succès !");

                return ServiceResult<bool>.Fail("Erreur lors de la suppression.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du département {Id}", id);
                return ServiceResult<bool>.Fail("Une erreur inattendue est survenue.");
            }
        }
    }
}
