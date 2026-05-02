using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GRH_SENTECH.Entity
{
    public class Poste
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "L'intitulé est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "L'intitulé doit contenir entre 2 et 100 caractères")]
        [Column("intitule")]
        public string Intitule { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le niveau hiérarchique est obligatoire")]
        [Range(1, 5, ErrorMessage = "Le niveau hiérarchique doit être entre 1 et 5")]
        [Column("niveau_hierarchique")]
        public int NiveauHierarchique { get; set; }

        [Required(ErrorMessage = "Le salaire minimum est obligatoire")]
        [Column("salaire_min")]
        public decimal SalaireMin { get; set; }

        [Required(ErrorMessage = "Le salaire maximum est obligatoire")]
        [Column("salaire_max")]
        public decimal SalaireMax { get; set; }

        [Column("date_creation")]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public ICollection<Contrat>? Contrats { get; set; } = new List<Contrat>();
    }
}
