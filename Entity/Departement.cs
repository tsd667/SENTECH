using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GRH_SENTECH.Entity
{
    public class Departement
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom doit contenir entre 3 et 50 caractères")]
        [Column("nom")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le code est obligatoire")]
        [StringLength(6, MinimumLength = 3, ErrorMessage = "Le code doit contenir entre 3 et 6 caractères")]
        [Column("code")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le budget est obligatoire")]
        [Column("budget")]
        public decimal Budget { get; set; }

        [Column("date_creation")]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        [Column("is_actif")]
        public bool IsActif { get; set; } = true;

        public ICollection<Employe>? Employes { get; set; } = new List<Employe>();
    }
}
