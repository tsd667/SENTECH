using GRH_SENTECH.Entity;

namespace GRH_SENTECH.Service
{
    public interface IDepartementService
    {
        IEnumerable<Departement> GetAllDepartements();
        Departement? GetDepartementById(int id);
        ServiceResult<Departement> CreateDepartement(Departement departement);
        ServiceResult<Departement> UpdateDepartement(Departement departement);
        ServiceResult<bool> DeleteDepartement(int id);
    }
}
