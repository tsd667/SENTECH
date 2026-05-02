using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GRH_SENTECH.Entity
{
    public class Evaluation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La période est obligatoire")]
        [StringLength(50, ErrorMessage = "La période ne peut pas dépasser 50 caractères")]
        [Column("periode")]
        public string Periode { get; set; } = string.Empty;

        [Required(ErrorMessage = "La note est obligatoire")]
        [Range(0, 20, ErrorMessage = "La note doit être entre 0 et 20")]
        [Column("note")]
        public decimal Note { get; set; }

        [StringLength(500, ErrorMessage = "Le commentaire ne peut pas dépasser 500 caractères")]
        [Column("commentaire")]
        public string? Commentaire { get; set; }

        [Required(ErrorMessage = "La date d'évaluation est obligatoire")]
        [Column("date_evaluation")]
        public DateTime DateEvaluation { get; set; } = DateTime.Now;

        // Clé étrangère vers Employe (many-to-one)
        [Column("employe_id")]
        public int EmployeId { get; set; }
        public Employe? Employe { get; set; }
    }
}
