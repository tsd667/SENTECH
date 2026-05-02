using System.ComponentModel.DataAnnotations;
using GRH_SENTECH.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GRH_SENTECH.ViewModels
{

    public class EmployeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le matricule est obligatoire")]
        [Display(Name = "Matricule")]
        public string Matricule { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [Display(Name = "Nom")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de naissance est obligatoire")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de naissance")]
        public DateTime DateNaissance { get; set; }

        [Display(Name = "Genre")]
        public Genre Genre { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date d'embauche")]
        public DateTime DateEmbauche { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Le département est obligatoire")]
        [Display(Name = "Département")]
        public int DepartementId { get; set; }

        public string? DepartementNom { get; set; }
        public string? ContratActifType { get; set; }
        public decimal? SalaireActuel { get; set; }
        public string? PosteActuel { get; set; }
        public bool IsActif { get; set; } = true;

        public IEnumerable<SelectListItem>? Departements { get; set; }

        public string NomComplet => $"{Prenom} {Nom}";

        public Employe ToEntity()
        {
            return new Employe
            {
                Id = this.Id,
                Matricule = this.Matricule,
                Nom = this.Nom,
                Prenom = this.Prenom,
                Email = this.Email,
                DateNaissance = this.DateNaissance,
                Genre = this.Genre,
                DateEmbauche = this.DateEmbauche,
                DepartementId = this.DepartementId,
                IsActif = this.IsActif
            };
        }

        public static EmployeViewModel FromEntity(Employe employe)
        {
            var contratActif = employe.Contrats?.FirstOrDefault(c => c.IsActif);
            return new EmployeViewModel
            {
                Id = employe.Id,
                Matricule = employe.Matricule,
                Nom = employe.Nom,
                Prenom = employe.Prenom,
                Email = employe.Email,
                DateNaissance = employe.DateNaissance,
                Genre = employe.Genre,
                DateEmbauche = employe.DateEmbauche,
                DepartementId = employe.DepartementId,
                DepartementNom = employe.Departement?.Nom,
                ContratActifType = contratActif?.TypeContrat.ToString(),
                SalaireActuel = contratActif?.SalaireBase,
                PosteActuel = contratActif?.Poste?.Intitule,
                IsActif = employe.IsActif
            };
        }
    }

    public class EmployeListViewModel
    {
        public IEnumerable<EmployeViewModel> Employes { get; set; } = new List<EmployeViewModel>();
        public int TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / Limit);
        public string? SearchTerm { get; set; }
        public int? DepartementFilter { get; set; }
        public IEnumerable<SelectListItem>? Departements { get; set; }
    }

    public class ContratViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le type de contrat est obligatoire")]
        [Display(Name = "Type de contrat")]
        public TypeContrat TypeContrat { get; set; }

        [Required(ErrorMessage = "La date de début est obligatoire")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de début")]
        public DateTime DateDebut { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [Display(Name = "Date de fin")]
        public DateTime? DateFin { get; set; }

        [Required(ErrorMessage = "Le salaire de base est obligatoire")]
        [Display(Name = "Salaire de base (FCFA)")]
        public decimal SalaireBase { get; set; }

        [Display(Name = "Période d'essai")]
        public bool PeriodeEssai { get; set; }

        public int EmployeId { get; set; }

        [Required(ErrorMessage = "Le poste est obligatoire")]
        [Display(Name = "Poste")]
        public int PosteId { get; set; }

        public IEnumerable<SelectListItem>? Postes { get; set; }
        public string? EmployeNomComplet { get; set; }
    }

    public class DashboardViewModel
    {
        public int TotalEmployes { get; set; }
        public int TotalDepartements { get; set; }
        public int TotalContratsCDI { get; set; }
        public int TotalContratsCDD { get; set; }
        public int TotalStages { get; set; }
        public int CongesEnAttente { get; set; }
        public IEnumerable<EmployeViewModel> DerniersEmployes { get; set; } = new List<EmployeViewModel>();
    }
}
