using System.ComponentModel.DataAnnotations;
using GRH_SENTECH.Entity;

namespace GRH_SENTECH.ViewModels
{
    public class DepartementViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom doit contenir entre 3 et 50 caractères")]
        [Display(Name = "Nom du département")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le code est obligatoire")]
        [StringLength(6, MinimumLength = 3, ErrorMessage = "Le code doit contenir entre 3 et 6 caractères")]
        [Display(Name = "Code")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le budget est obligatoire")]
        [Display(Name = "Budget (FCFA)")]
        public decimal Budget { get; set; }

        [Display(Name = "Actif")]
        public bool IsActif { get; set; } = true;

        public int NombreEmployes { get; set; }
        public decimal TotalSalaires { get; set; }
        public DateTime DateCreation { get; set; }

        public Departement ToEntity()
        {
            return new Departement
            {
                Id = this.Id,
                Nom = this.Nom,
                Code = this.Code,
                Budget = this.Budget,
                IsActif = this.IsActif,
                DateCreation = this.DateCreation == default ? DateTime.Now : this.DateCreation
            };
        }

        public static DepartementViewModel FromEntity(Departement dept)
        {
            return new DepartementViewModel
            {
                Id = dept.Id,
                Nom = dept.Nom,
                Code = dept.Code,
                Budget = dept.Budget,
                IsActif = dept.IsActif,
                NombreEmployes = dept.Employes?.Count(e => e.IsActif) ?? 0,
                DateCreation = dept.DateCreation
            };
        }
    }
}
