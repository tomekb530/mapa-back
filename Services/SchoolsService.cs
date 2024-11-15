using mapa_back.Exceptions;
using mapa_back.Models;
using mapa_back.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace mapa_back.Services
{
    public class SchoolsService : ISchoolsService
    {
        private readonly DatabaseContext _dbContext;

        public SchoolsService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> GetSchoolsCount()
        {
            try
            {
                long schoolsNumber = await _dbContext.Schools.CountAsync();
                return schoolsNumber;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Unexpected error occurred while trying to count elements in database");
            }
        }
        private bool ValidatePageParametres(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return false;
            }
            return true;
        }
        private List<SchoolDTO> GetSchoolsAsBusinessData(List<School> schools)
        {
            List<SchoolDTO> businessDataList = new List<SchoolDTO>();
            foreach (var school in schools)
            {
                SchoolDTO dataFromJson = JsonConvert.DeserializeObject<SchoolDTO>(school.BusinessData);
                dataFromJson.Geography = new GeographyDTO { X = school.Geography.X, Y = school.Geography.Y };
                dataFromJson.Id = school.Id;
                businessDataList.Add(dataFromJson);
            }
            return businessDataList;
        }
        public async Task<List<SchoolDTO>> GetSchoolsPage(int size, int pageNumber)
        {
            if (!ValidatePageParametres(pageNumber, size))
            {
                throw new ArgumentException("Parametres not valid");
            }
            try
            {
                List<School> schoolsPage = await _dbContext.Schools.Where(p => true).Skip((pageNumber -1) * size).Take(size).ToListAsync();
                return GetSchoolsAsBusinessData(schoolsPage);
            }
            catch(Exception)
            {
                throw new Exception("Unexpected error occurred while trying to get school page from database");
            }
        }
        public async Task DeleteSingleSchool(int id)
        {
            try
            {
                School school = await _dbContext.Schools.FirstOrDefaultAsync(p => p.Id == id);
                if(school == null)
                {
                    throw new DatabaseException($"Couldnt find school with given Id: {id}");
                }
                _dbContext.Schools.Remove(school);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DatabaseException($"Unexpected eror occurred while trying to delete school with given Id: {id} from database");
            }
        }
        public async Task DeleteManySchools(List<int> ids)
        {
            try
            {
                List<School> schools = await _dbContext.Schools.Where(school => ids.Contains(school.Id)).ToListAsync();
                if (!schools.Any())
                {
                    throw new DatabaseException("No schools found with the provided IDs.");
                }
                _dbContext.Schools.RemoveRange(schools);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw new DatabaseException("An unexpected error occurred while deleting schools from the database.");
            }
        }
    }
}
