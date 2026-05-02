using GRH_SENTECH.Entity;

namespace GRH_SENTECH.Repository
{
    public interface IContratRepository
    {
        IEnumerable<Contrat> FindAll();
        Contrat? FindById(int id);
        Contrat? FindContratActif(int employeId);
        bool Add(Contrat contrat);
        bool Update(Contrat contrat);
        bool ClotureContrat(int contratId);
        IEnumerable<Contrat> FindByEmploye(int employeId);
    }
}
