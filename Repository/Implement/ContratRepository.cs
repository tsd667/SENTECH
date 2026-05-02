using GRH_SENTECH.Data;
using GRH_SENTECH.Entity;
using Microsoft.EntityFrameworkCore;

namespace GRH_SENTECH.Repository.Implement
{
    public class ContratRepository : IContratRepository
    {
        private readonly GrhDbContext _context;

        public ContratRepository(GrhDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Contrat> FindAll()
        {
            return _context.Contrats
                .Include(c => c.Employe)
                .Include(c => c.Poste)
                .OrderByDescending(c => c.DateDebut)
                .ToList();
        }

        public Contrat? FindById(int id)
        {
            return _context.Contrats
                .Include(c => c.Employe)
                .Include(c => c.Poste)
                .SingleOrDefault(c => c.Id == id);
        }

        public Contrat? FindContratActif(int employeId)
        {
            return _context.Contrats
                .Include(c => c.Poste)
                .SingleOrDefault(c => c.EmployeId == employeId && c.IsActif);
        }

        public bool Add(Contrat contrat)
        {
            _context.Contrats.Add(contrat);
            return _context.SaveChanges() > 0;
        }

        public bool Update(Contrat contrat)
        {
            _context.Contrats.Update(contrat);
            return _context.SaveChanges() > 0;
        }

        public bool ClotureContrat(int contratId)
        {
            var contrat = _context.Contrats.Find(contratId);
            if (contrat == null) return false;
            contrat.IsActif = false;
            contrat.DateFin = DateTime.Now;
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Contrat> FindByEmploye(int employeId)
        {
            return _context.Contrats
                .Include(c => c.Poste)
                .Where(c => c.EmployeId == employeId)
                .OrderByDescending(c => c.DateDebut)
                .ToList();
        }
    }
}
