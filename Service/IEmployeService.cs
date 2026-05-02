using GRH_SENTECH.Entity;

namespace GRH_SENTECH.Service
{
    public interface IEmployeService
    {
        IEnumerable<Employe> GetAllEmployes(int offset = 0, int limit = 10);
        Employe? GetEmployeById(int id);
        ServiceResult<Employe> CreateEmploye(Employe employe);
        ServiceResult<Employe> UpdateEmploye(Employe employe);
        ServiceResult<bool> DeleteEmploye(int id);
        ServiceResult<Contrat> AjouterContrat(Contrat contrat);
        ServiceResult<bool> TransfertEmploye(int employeId, int nouveauDepartementId, int nouveauPosteId, decimal nouveauSalaire);
        int CountEmployes();
        IEnumerable<Employe> GetEmployesByDepartement(int departementId);
    }
}
