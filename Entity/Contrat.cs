using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GRH_SENTECH.Entity
{
    public class Contrat
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le type de contrat est obligatoire")]
        [Column("type_contrat")]
        public TypeContrat TypeContrat { get; set; }

        [Required(ErrorMessage = "La date de début est obligatoire")]
        [Column("date_debut")]
        public DateTime DateDebut { get; set; }

        [Column("date_fin")]
        public DateTime? DateFin { get; set; }

        [Required(ErrorMessage = "Le salaire de base est obligatoire")]
        [Column("salaire_base")]
        public decimal SalaireBase { get; set; }

        [Column("periode_essai")]
        public bool PeriodeEssai { get; set; } = false;

        [Column("is_actif")]
        public bool IsActif { get; set; } = true;

        [Column("date_creation")]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        [Column("employe_id")]
        public int EmployeId { get; set; }
        public Employe? Employe { get; set; }

        [Column("poste_id")]
        public int PosteId { get; set; }
        public Poste? Poste { get; set; }
    }
}
