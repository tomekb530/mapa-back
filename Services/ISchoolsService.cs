using mapa_back.Models;
using mapa_back.Models.DTO;

namespace mapa_back.Services
{
    public interface ISchoolsService
    {
        Task<List<SchoolDTO>> GetSchoolsPage(int size, int pageNumber);
        Task DeleteSingleSchool(int id);
        Task DeleteManySchools(List<int> ids);
        Task<ChangedSchools> GetChangedSchoolsList(int size, int pageNumber);
        Task<long> GetSchoolsCount();
	}
}
