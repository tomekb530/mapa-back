using mapa_back.Data;
using mapa_back.Models.DTO;

namespace mapa_back.Services
{
    public interface ISchoolsService
    {
        Task<List<SchoolDTO>> GetSchoolsPage(int size, int pageNumber);
        Task DeleteSingleSchool(int id);
        Task DeleteManySchools(List<int> ids);
        Task<ChangedSchoolsResponse> GetChangedSchoolsList(int size, int pageNumber);
        Task<long> GetSchoolsCount();
	}
}
