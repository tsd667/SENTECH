using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GRH_SENTECH.Entity
{
    public class Conge
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le type de congé est obligatoire")]
        [Column("type_conge")]
        public TypeConge TypeConge { get; set; }

        [Required(ErrorMessage = "La date de début est obligatoire")]
        [Column("date_debut")]
        public DateTime DateDebut { get; set; }

        [Required(ErrorMessage = "La date de fin est obligatoire")]
        [Column("date_fin")]
        public DateTime DateFin { get; set; }

        [Column("statut")]
        public StatutConge Statut { get; set; } = StatutConge.EnAttente;

        [StringLength(300, ErrorMessage = "Le motif ne peut pas dépasser 300 caractères")]
        [Column("motif")]
        public string? Motif { get; set; }

        [Column("date_creation")]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        // Clé étrangère vers Employe (many-to-one)
        [Column("employe_id")]
        public int EmployeId { get; set; }
        public Employe? Employe { get; set; }
    }
}
