using GRH_SENTECH.Entity;

namespace GRH_SENTECH.Repository
{
       public interface IEmployeRepository
    {
        IEnumerable<Employe> FindAll(int offset = 0, int limit = 10);
        Employe? FindById(int id);
        Employe? FindByMatricule(string matricule);
        bool Add(Employe employe);
        bool Update(Employe employe);
        bool Delete(int id);
        int Count();
        IEnumerable<Employe> FindByDepartement(int departementId);
    }
}
