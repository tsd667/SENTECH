using GRH_SENTECH.Data;
using GRH_SENTECH.Entity;
using Microsoft.EntityFrameworkCore;

namespace GRH_SENTECH.Repository.Implement
{
    public class DepartementRepository : IDepartementRepository
    {
        private readonly GrhDbContext _context;

        public DepartementRepository(GrhDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Departement> FindAll()
        {
            return _context.Departements
                .Include(d => d.Employes)
                .Where(d => d.IsActif)
                .OrderBy(d => d.Nom)
                .ToList();
        }

        public Departement? FindById(int id)
        {
            return _context.Departements
                .Include(d => d.Employes!.Where(e => e.IsActif))
                .SingleOrDefault(d => d.Id == id);
        }

        public bool Add(Departement departement)
        {
            _context.Departements.Add(departement);
            return _context.SaveChanges() > 0;
        }

        public bool Update(Departement departement)
        {
            _context.Departements.Update(departement);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var dept = _context.Departements.Find(id);
            if (dept == null) return false;
            dept.IsActif = false;
            return _context.SaveChanges() > 0;
        }

        public int Count()
        {
            return _context.Departements.Count(d => d.IsActif);
        }
    }
}
