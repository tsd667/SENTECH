using Microsoft.EntityFrameworkCore;
using GRH_SENTECH.Entity;

namespace GRH_SENTECH.Data
{
    public class GrhDbContext : DbContext
    {
        public GrhDbContext(DbContextOptions<GrhDbContext> options) : base(options)
        {
        }

        public DbSet<Departement> Departements { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Poste> Postes { get; set; }
        public DbSet<Contrat> Contrats { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Conge> Conges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Departement>(entity =>
            {
                entity.ToTable("departements");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(d => d.Nom).HasColumnName("nom").IsRequired().HasMaxLength(50);
                entity.Property(d => d.Code).HasColumnName("code").IsRequired().HasMaxLength(6);
                entity.Property(d => d.Budget).HasColumnName("budget").HasColumnType("decimal(18,2)");
                entity.Property(d => d.IsActif).HasColumnName("is_actif").HasDefaultValue(true);
                entity.Property(d => d.DateCreation).HasColumnName("date_creation");

                entity.HasIndex(d => d.Code).IsUnique();
            });

            modelBuilder.Entity<Poste>(entity =>
            {
                entity.ToTable("postes");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(p => p.Intitule).HasColumnName("intitule").IsRequired().HasMaxLength(100);
                entity.Property(p => p.NiveauHierarchique).HasColumnName("niveau_hierarchique");
                entity.Property(p => p.SalaireMin).HasColumnName("salaire_min").HasColumnType("decimal(18,2)");
                entity.Property(p => p.SalaireMax).HasColumnName("salaire_max").HasColumnType("decimal(18,2)");
                entity.Property(p => p.DateCreation).HasColumnName("date_creation");
            });

            modelBuilder.Entity<Employe>(entity =>
            {
                entity.ToTable("employes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Matricule).HasColumnName("matricule").IsRequired().HasMaxLength(20);
                entity.Property(e => e.Nom).HasColumnName("nom").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Prenom).HasColumnName("prenom").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(150);
                entity.Property(e => e.DateNaissance).HasColumnName("date_naissance");
                entity.Property(e => e.DateEmbauche).HasColumnName("date_embauche");
                entity.Property(e => e.Genre).HasColumnName("genre");
                entity.Property(e => e.IsActif).HasColumnName("is_actif").HasDefaultValue(true);
                entity.Property(e => e.DepartementId).HasColumnName("departement_id");

                entity.HasIndex(e => e.Matricule).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();


                entity.HasOne(e => e.Departement)
                      .WithMany(d => d.Employes)
                      .HasForeignKey(e => e.DepartementId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Contrat>(entity =>
            {
                entity.ToTable("contrats");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(c => c.TypeContrat).HasColumnName("type_contrat");
                entity.Property(c => c.DateDebut).HasColumnName("date_debut");
                entity.Property(c => c.DateFin).HasColumnName("date_fin");
                entity.Property(c => c.SalaireBase).HasColumnName("salaire_base").HasColumnType("decimal(18,2)");
                entity.Property(c => c.PeriodeEssai).HasColumnName("periode_essai").HasDefaultValue(false);
                entity.Property(c => c.IsActif).HasColumnName("is_actif").HasDefaultValue(true);
                entity.Property(c => c.DateCreation).HasColumnName("date_creation");
                entity.Property(c => c.EmployeId).HasColumnName("employe_id");
                entity.Property(c => c.PosteId).HasColumnName("poste_id");

                entity.HasOne(c => c.Employe)
                      .WithMany(e => e.Contrats)
                      .HasForeignKey(c => c.EmployeId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Poste)
                      .WithMany(p => p.Contrats)
                      .HasForeignKey(c => c.PosteId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.ToTable("evaluations");
                entity.HasKey(ev => ev.Id);
                entity.Property(ev => ev.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(ev => ev.Periode).HasColumnName("periode").IsRequired().HasMaxLength(50);
                entity.Property(ev => ev.Note).HasColumnName("note").HasColumnType("decimal(4,2)");
                entity.Property(ev => ev.Commentaire).HasColumnName("commentaire").HasMaxLength(500);
                entity.Property(ev => ev.DateEvaluation).HasColumnName("date_evaluation");
                entity.Property(ev => ev.EmployeId).HasColumnName("employe_id");

                entity.HasOne(ev => ev.Employe)
                      .WithMany(e => e.Evaluations)
                      .HasForeignKey(ev => ev.EmployeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Conge>(entity =>
            {
                entity.ToTable("conges");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(c => c.TypeConge).HasColumnName("type_conge");
                entity.Property(c => c.DateDebut).HasColumnName("date_debut");
                entity.Property(c => c.DateFin).HasColumnName("date_fin");
                entity.Property(c => c.Statut).HasColumnName("statut");
                entity.Property(c => c.Motif).HasColumnName("motif").HasMaxLength(300);
                entity.Property(c => c.DateCreation).HasColumnName("date_creation");
                entity.Property(c => c.EmployeId).HasColumnName("employe_id");

                entity.HasOne(c => c.Employe)
                      .WithMany(e => e.Conges)
                      .HasForeignKey(c => c.EmployeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departement>().HasData(
                new Departement { Id = 1, Nom = "Direction Générale", Code = "DG", Budget = 5000000, DateCreation = new DateTime(2020, 1, 1), IsActif = true },
                new Departement { Id = 2, Nom = "Informatique", Code = "INFO", Budget = 3000000, DateCreation = new DateTime(2020, 1, 1), IsActif = true },
                new Departement { Id = 3, Nom = "Ressources Humaines", Code = "RH", Budget = 2000000, DateCreation = new DateTime(2020, 1, 1), IsActif = true },
                new Departement { Id = 4, Nom = "Finance et Comptabilité", Code = "FIN", Budget = 2500000, DateCreation = new DateTime(2020, 1, 1), IsActif = true },
                new Departement { Id = 5, Nom = "Marketing", Code = "MKT", Budget = 1500000, DateCreation = new DateTime(2020, 1, 1), IsActif = true }
            );

            modelBuilder.Entity<Poste>().HasData(
                new Poste { Id = 1, Intitule = "Directeur Général", NiveauHierarchique = 5, SalaireMin = 800000, SalaireMax = 1500000, DateCreation = new DateTime(2020, 1, 1) },
                new Poste { Id = 2, Intitule = "Développeur Senior", NiveauHierarchique = 3, SalaireMin = 400000, SalaireMax = 700000, DateCreation = new DateTime(2020, 1, 1) },
                new Poste { Id = 3, Intitule = "Développeur Junior", NiveauHierarchique = 1, SalaireMin = 200000, SalaireMax = 350000, DateCreation = new DateTime(2020, 1, 1) },
                new Poste { Id = 4, Intitule = "Chef de Projet", NiveauHierarchique = 4, SalaireMin = 500000, SalaireMax = 800000, DateCreation = new DateTime(2020, 1, 1) },
                new Poste { Id = 5, Intitule = "Responsable RH", NiveauHierarchique = 3, SalaireMin = 350000, SalaireMax = 600000, DateCreation = new DateTime(2020, 1, 1) },
                new Poste { Id = 6, Intitule = "Comptable", NiveauHierarchique = 2, SalaireMin = 250000, SalaireMax = 450000, DateCreation = new DateTime(2020, 1, 1) },
                new Poste { Id = 7, Intitule = "Chargé Marketing", NiveauHierarchique = 2, SalaireMin = 280000, SalaireMax = 480000, DateCreation = new DateTime(2020, 1, 1) }
            );

            modelBuilder.Entity<Employe>().HasData(
                new Employe { Id = 1, Matricule = "EMP001", Nom = "Diallo", Prenom = "Mamadou", Email = "m.diallo@sentech.sn", DateNaissance = new DateTime(1985, 3, 15), Genre = Genre.Masculin, DateEmbauche = new DateTime(2020, 2, 1), DepartementId = 1, IsActif = true },
                new Employe { Id = 2, Matricule = "EMP002", Nom = "Ndiaye", Prenom = "Fatou", Email = "f.ndiaye@sentech.sn", DateNaissance = new DateTime(1990, 7, 22), Genre = Genre.Feminin, DateEmbauche = new DateTime(2020, 3, 1), DepartementId = 2, IsActif = true },
                new Employe { Id = 3, Matricule = "EMP003", Nom = "Sow", Prenom = "Ibrahima", Email = "i.sow@sentech.sn", DateNaissance = new DateTime(1988, 11, 5), Genre = Genre.Masculin, DateEmbauche = new DateTime(2021, 1, 15), DepartementId = 2, IsActif = true },
                new Employe { Id = 4, Matricule = "EMP004", Nom = "Fall", Prenom = "Aissatou", Email = "a.fall@sentech.sn", DateNaissance = new DateTime(1992, 4, 10), Genre = Genre.Feminin, DateEmbauche = new DateTime(2021, 6, 1), DepartementId = 3, IsActif = true },
                new Employe { Id = 5, Matricule = "EMP005", Nom = "Ba", Prenom = "Ousmane", Email = "o.ba@sentech.sn", DateNaissance = new DateTime(1987, 9, 30), Genre = Genre.Masculin, DateEmbauche = new DateTime(2020, 5, 15), DepartementId = 4, IsActif = true },
                new Employe { Id = 6, Matricule = "EMP006", Nom = "Sarr", Prenom = "Mariama", Email = "m.sarr@sentech.sn", DateNaissance = new DateTime(1993, 1, 18), Genre = Genre.Feminin, DateEmbauche = new DateTime(2022, 1, 10), DepartementId = 5, IsActif = true },
                new Employe { Id = 7, Matricule = "EMP007", Nom = "Gueye", Prenom = "Cheikh", Email = "c.gueye@sentech.sn", DateNaissance = new DateTime(1991, 6, 25), Genre = Genre.Masculin, DateEmbauche = new DateTime(2021, 9, 1), DepartementId = 2, IsActif = true },
                new Employe { Id = 8, Matricule = "EMP008", Nom = "Mbaye", Prenom = "Rokhaya", Email = "r.mbaye@sentech.sn", DateNaissance = new DateTime(1986, 12, 8), Genre = Genre.Feminin, DateEmbauche = new DateTime(2020, 8, 1), DepartementId = 3, IsActif = true },
                new Employe { Id = 9, Matricule = "EMP009", Nom = "Cisse", Prenom = "Abdoulaye", Email = "a.cisse@sentech.sn", DateNaissance = new DateTime(1989, 2, 14), Genre = Genre.Masculin, DateEmbauche = new DateTime(2022, 4, 15), DepartementId = 4, IsActif = true },
                new Employe { Id = 10, Matricule = "EMP010", Nom = "Toure", Prenom = "Khadija", Email = "k.toure@sentech.sn", DateNaissance = new DateTime(1994, 8, 20), Genre = Genre.Feminin, DateEmbauche = new DateTime(2023, 1, 5), DepartementId = 5, IsActif = true }
            );

            modelBuilder.Entity<Contrat>().HasData(
                new Contrat { Id = 1, TypeContrat = TypeContrat.CDI, DateDebut = new DateTime(2020, 2, 1), DateFin = null, SalaireBase = 1000000, PeriodeEssai = false, IsActif = true, EmployeId = 1, PosteId = 1, DateCreation = new DateTime(2020, 2, 1) },
                new Contrat { Id = 2, TypeContrat = TypeContrat.CDI, DateDebut = new DateTime(2020, 3, 1), DateFin = null, SalaireBase = 550000, PeriodeEssai = false, IsActif = true, EmployeId = 2, PosteId = 2, DateCreation = new DateTime(2020, 3, 1) },
                new Contrat { Id = 3, TypeContrat = TypeContrat.CDD, DateDebut = new DateTime(2021, 1, 15), DateFin = new DateTime(2023, 1, 15), SalaireBase = 280000, PeriodeEssai = true, IsActif = false, EmployeId = 3, PosteId = 3, DateCreation = new DateTime(2021, 1, 15) },
                new Contrat { Id = 4, TypeContrat = TypeContrat.CDI, DateDebut = new DateTime(2021, 1, 15), DateFin = null, SalaireBase = 400000, PeriodeEssai = false, IsActif = true, EmployeId = 3, PosteId = 2, DateCreation = new DateTime(2023, 1, 16) },
                new Contrat { Id = 5, TypeContrat = TypeContrat.CDI, DateDebut = new DateTime(2021, 6, 1), DateFin = null, SalaireBase = 450000, PeriodeEssai = false, IsActif = true, EmployeId = 4, PosteId = 5, DateCreation = new DateTime(2021, 6, 1) },
                new Contrat { Id = 6, TypeContrat = TypeContrat.CDI, DateDebut = new DateTime(2020, 5, 15), DateFin = null, SalaireBase = 380000, PeriodeEssai = false, IsActif = true, EmployeId = 5, PosteId = 6, DateCreation = new DateTime(2020, 5, 15) },
                new Contrat { Id = 7, TypeContrat = TypeContrat.Stage, DateDebut = new DateTime(2022, 1, 10), DateFin = new DateTime(2022, 7, 10), SalaireBase = 150000, PeriodeEssai = false, IsActif = false, EmployeId = 6, PosteId = 7, DateCreation = new DateTime(2022, 1, 10) },
                new Contrat { Id = 8, TypeContrat = TypeContrat.CDI, DateDebut = new DateTime(2022, 7, 11), DateFin = null, SalaireBase = 320000, PeriodeEssai = false, IsActif = true, EmployeId = 6, PosteId = 7, DateCreation = new DateTime(2022, 7, 11) }
            );
        }
    }
}
