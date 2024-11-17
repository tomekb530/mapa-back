using mapa_back.Models;
using mapa_back.Models.DTO;

namespace mapa_back.Services
{
    public interface ISchoolsService
    {
        Task<long> GetSchoolsCount();
        Task<List<SchoolDTO>> GetSchoolsPage(int size, int pageNumber);
        Task DeleteSingleSchool(int id);
        Task DeleteManySchools(List<int> ids);
        Task<List<ChangedSchool>> GetChangedSchoolsList(int size, int pageNumber);
        Task<long> GetChangesCount();
    }
}
