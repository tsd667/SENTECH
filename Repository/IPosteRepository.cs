using GRH_SENTECH.Entity;

namespace GRH_SENTECH.Repository
{
    public interface IPosteRepository
    {
        IEnumerable<Poste> FindAll();
        Poste? FindById(int id);
        bool Add(Poste poste);
        bool Update(Poste poste);
    }
}
