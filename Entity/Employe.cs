using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GRH_SENTECH.Entity
{
    public class Employe
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le matricule est obligatoire")]
        [StringLength(20, ErrorMessage = "Le matricule ne peut pas dépasser 20 caractères")]
        [Column("matricule")]
        public string Matricule { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        [Column("nom")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères")]
        [Column("prenom")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de naissance est obligatoire")]
        [Column("date_naissance")]
        public DateTime DateNaissance { get; set; }

        [Column("genre")]
        public Genre Genre { get; set; }

        [Column("date_embauche")]
        public DateTime DateEmbauche { get; set; } = DateTime.Now;

        [Column("is_actif")]
        public bool IsActif { get; set; } = true;

        [Column("departement_id")]
        public int DepartementId { get; set; }

        public Departement? Departement { get; set; }

        public ICollection<Contrat>? Contrats { get; set; } = new List<Contrat>();

        public ICollection<Evaluation>? Evaluations { get; set; } = new List<Evaluation>();

        public ICollection<Conge>? Conges { get; set; } = new List<Conge>();

        // Propriété calculée - nom complet
        public string NomComplet => $"{Prenom} {Nom}";
    }
}
