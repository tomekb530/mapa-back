using mapa_back.Data;
using mapa_back.Models;
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
        Task<ChangedSchool> GetSingleChangedSchool(int id);
        Task<bool> PostSingleSchool(School school);
        Task<bool> PostManySchools(List<School> schools);
        Task<bool> UpdateSingleSchool(School school);
		Task<bool> UpdateManySchools(List<School> schools);

	}
}
