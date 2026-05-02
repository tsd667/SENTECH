using GRH_SENTECH.Data;
using GRH_SENTECH.Entity;
using Microsoft.EntityFrameworkCore;

namespace GRH_SENTECH.Repository.Implement
{
    public class PosteRepository : IPosteRepository
    {
        private readonly GrhDbContext _context;

        public PosteRepository(GrhDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Poste> FindAll()
        {
            return _context.Postes.OrderBy(p => p.NiveauHierarchique).ToList();
        }

        public Poste? FindById(int id)
        {
            return _context.Postes.SingleOrDefault(p => p.Id == id);
        }

        public bool Add(Poste poste)
        {
            _context.Postes.Add(poste);
            return _context.SaveChanges() > 0;
        }

        public bool Update(Poste poste)
        {
            _context.Postes.Update(poste);
            return _context.SaveChanges() > 0;
        }
    }

    public class CongeRepository : ICongeRepository
    {
        private readonly GrhDbContext _context;

        public CongeRepository(GrhDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Conge> FindAll()
        {
            return _context.Conges
                .Include(c => c.Employe)
                .OrderByDescending(c => c.DateCreation)
                .ToList();
        }

        public Conge? FindById(int id)
        {
            return _context.Conges
                .Include(c => c.Employe)
                .SingleOrDefault(c => c.Id == id);
        }

        public IEnumerable<Conge> FindByEmploye(int employeId, int annee)
        {
            return _context.Conges
                .Where(c => c.EmployeId == employeId && c.DateDebut.Year == annee)
                .OrderByDescending(c => c.DateDebut)
                .ToList();
        }

        public bool Add(Conge conge)
        {
            _context.Conges.Add(conge);
            return _context.SaveChanges() > 0;
        }

        public bool Update(Conge conge)
        {
            _context.Conges.Update(conge);
            return _context.SaveChanges() > 0;
        }

        public int CountJoursCongeAnnuel(int employeId, int annee)
        {
            var conges = _context.Conges
                .Where(c => c.EmployeId == employeId
                         && c.TypeConge == TypeConge.Annuel
                         && c.DateDebut.Year == annee
                         && c.Statut != StatutConge.Refuse
                         && c.Statut != StatutConge.Annule)
                .ToList();

            return conges.Sum(c => (c.DateFin - c.DateDebut).Days + 1);
        }
    }
}
