using GRH_SENTECH.Data;
using GRH_SENTECH.Entity;
using Microsoft.EntityFrameworkCore;

namespace GRH_SENTECH.Repository.Implement
{

    public class EmployeRepository : IEmployeRepository
    {
        private readonly GrhDbContext _context;

        public EmployeRepository(GrhDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Employe> FindAll(int offset = 0, int limit = 10)
        {
            
            return _context.Employes
                .Include(e => e.Departement)
                .Include(e => e.Contrats)
                .Where(e => e.IsActif)
                .OrderByDescending(e => e.DateEmbauche)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        public Employe? FindById(int id)
        {
            return _context.Employes
                .Include(e => e.Departement)
                .Include(e => e.Contrats!.Where(c => c.IsActif))
                    .ThenInclude(c => c.Poste)
                .Include(e => e.Evaluations)
                .Include(e => e.Conges)
                .SingleOrDefault(e => e.Id == id);
        }

        public Employe? FindByMatricule(string matricule)
        {
            return _context.Employes
                .Include(e => e.Departement)
                .SingleOrDefault(e => e.Matricule == matricule);
        }

        public bool Add(Employe employe)
        {
            _context.Employes.Add(employe);
            return _context.SaveChanges() > 0;
        }

        public bool Update(Employe employe)
        {
            _context.Employes.Update(employe);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var employe = _context.Employes.Find(id);
            if (employe == null) return false;
            employe.IsActif = false; 
            return _context.SaveChanges() > 0;
        }

        public int Count()
        {
            return _context.Employes.Count(e => e.IsActif);
        }

        public IEnumerable<Employe> FindByDepartement(int departementId)
        {
            return _context.Employes
                .Include(e => e.Contrats!.Where(c => c.IsActif))
                    .ThenInclude(c => c.Poste)
                .Where(e => e.DepartementId == departementId && e.IsActif)
                .OrderBy(e => e.Nom)
                .ToList();
        }
    }
}
