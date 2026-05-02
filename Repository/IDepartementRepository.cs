using GRH_SENTECH.Entity;

namespace GRH_SENTECH.Repository
{
    public interface IDepartementRepository
    {
        IEnumerable<Departement> FindAll();
        Departement? FindById(int id);
        bool Add(Departement departement);
        bool Update(Departement departement);
        bool Delete(int id);
        int Count();
    }
}
