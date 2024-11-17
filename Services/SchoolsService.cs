using AutoMapper;
using mapa_back.Exceptions;
using mapa_back.Models;
using mapa_back.Models.DTO;
using mapa_back.Models.RSPOApi;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace mapa_back.Services
{
    public class SchoolsService : ISchoolsService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;

        public SchoolsService(DatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<long> GetSchoolsCount()
        {
            try
            {
                long schoolsNumber = await _dbContext.SchoolsFromRSPO.CountAsync();
                return schoolsNumber;
            }
            catch (Exception)
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
        private List<SchoolDTO> GetSchoolsAsBusinessData(List<SchoolFromRSPO> schools)
        {
            List<SchoolDTO> businessDataList = new List<SchoolDTO>();
            foreach (var school in schools)
            {
                SchoolDTO dataFromJson = JsonConvert.DeserializeObject<SchoolDTO>(school.BusinessData) ?? throw new SchoolServiceException("Error while trying to deserialize object");
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
                List<SchoolFromRSPO> schoolsPage = await _dbContext.SchoolsFromRSPO.Where(p => true).Skip((pageNumber -1) * size).Take(size).ToListAsync();
                return GetSchoolsAsBusinessData(schoolsPage);
            }
            catch(SchoolServiceException)
            {
                throw;
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
                SchoolFromMap school = await _dbContext.SchoolsFromMapDatabase.FirstOrDefaultAsync(p => p.Id == id) ?? throw new DatabaseException($"Couldnt find school with given Id: {id}");
                _dbContext.SchoolsFromMapDatabase.Remove(school);
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
                List<SchoolFromMap> schools = await _dbContext.SchoolsFromMapDatabase.Where(school => ids.Contains(school.Id)).ToListAsync();
                if (!schools.Any())
                {
                    throw new DatabaseException("No schools found with the provided IDs.");
                }
                _dbContext.SchoolsFromMapDatabase.RemoveRange(schools);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw new DatabaseException("An unexpected error occurred while deleting schools from the database.");
            }
        }
        private ChangedSchool GetChangedSchool(SchoolFromRSPO schoolFromRSPO, SchoolFromMap schoolFromMap)
        {
            try
            {
                BusinessDataDTO schoolFromRSPOBusinessData = JsonConvert.DeserializeObject<BusinessDataDTO>(schoolFromRSPO.BusinessData) ?? throw new SchoolServiceException("Deserialized schoolFromRSPO data is null.");
                BusinessDataDTO schoolFroMMapBusinessData = JsonConvert.DeserializeObject<BusinessDataDTO>(schoolFromMap.BusinessData) ?? throw new SchoolServiceException("Deserialized schoolFromRSPO data is null.");
                SchoolDTO schoolFromRSPODTO = _mapper.Map<SchoolDTO>(schoolFromRSPOBusinessData, opts =>
                {
                    opts.Items["id"] = schoolFromRSPO.Id;
                    opts.Items["geography"] = schoolFromRSPO.Geography;
                });
                SchoolDTO schoolFromMapDTO = _mapper.Map<SchoolDTO>(schoolFroMMapBusinessData, opts =>
                {
                    opts.Items["id"] = schoolFromMap.Id;
                    opts.Items["geography"] = schoolFromMap.Geography;
                });
                return new ChangedSchool { SchoolsBeforeChanges = schoolFromMapDTO, SchoolsAfterChanges = schoolFromRSPODTO };
            }
            catch(Exception)
            {
                throw new SchoolServiceException("Erorr occurred while trying to map schools");
            }
        }
        public async Task<long> GetChangesCount()
        {
            try
            {
                long result = _dbContext.SchoolsFromMapDatabase.FromSqlRaw(@"SELECT COUNT(t1.id) as Id
FROM private_schools t1 LEFT JOIN rspo_cache t2 ON t1.rspo_number = t2.rspo_number
WHERE t1.business_data::jsonb <> t2.business_data::jsonb or t1.geography <> t2.geography").Select(s => s.Id).First();

                return result;
            }
            catch (Exception)
            {
                throw new DatabaseException("Unexpected error occurred while trying to count elements in database");
            }
        }
        public async Task<List<ChangedSchool>> GetChangedSchoolsList(int size, int pageNumber)
        {
            try
            {
                if (!ValidatePageParametres(pageNumber, size))
                {
                    throw new ArgumentException("Parametres not valid");
                }
                int offset = (pageNumber - 1) * size;
                string query = @"SELECT t1.id, t1.rspo_number, t1.business_data, t1.geography
FROM private_schools t1 LEFT JOIN rspo_cache t2 ON t1.rspo_number = t2.rspo_number
WHERE t1.business_data::jsonb <> t2.business_data::jsonb or t1.geography <> t2.geography
LIMIT @size OFFSET @offset;";
                List<SchoolFromMap> oldDataSchools = await _dbContext.SchoolsFromMapDatabase
                    .FromSqlRaw(query, new NpgsqlParameter("@size", size), new NpgsqlParameter("@offset", offset)).ToListAsync();
                if(!oldDataSchools.Any())
                {
                    return new List<ChangedSchool>();
                }
                List<SchoolFromRSPO> newDataSchools = await _dbContext.SchoolsFromRSPO
                    .Where(p => oldDataSchools.Select(s => s.RspoNumber).Contains(p.RspoNumber)).ToListAsync();
                var newDataSchoolsDict = newDataSchools.ToDictionary(s => s.RspoNumber);
                int i = 0;
                List<ChangedSchool> changedSchools = oldDataSchools.Select(oldSchool =>
                {
                    newDataSchoolsDict.TryGetValue(oldSchool.RspoNumber, out var schoolFromRSPO);
                    return GetChangedSchool(schoolFromRSPO, oldSchool);
                    
                }).Where(changedSchool => changedSchool != null).ToList();
                return changedSchools;

            }
            catch(SchoolServiceException)
            {
                throw;
            }
            catch(Exception)
            {
                throw new DatabaseException("An unexpected error occurred while trying to get data from database");
            }
            
        }
    }
}
