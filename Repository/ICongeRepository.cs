using GRH_SENTECH.Entity;

namespace GRH_SENTECH.Repository
{
    public interface ICongeRepository
    {
        IEnumerable<Conge> FindAll();
        Conge? FindById(int id);
        IEnumerable<Conge> FindByEmploye(int employeId, int annee);
        bool Add(Conge conge);
        bool Update(Conge conge);
        int CountJoursCongeAnnuel(int employeId, int annee);
    }
}
