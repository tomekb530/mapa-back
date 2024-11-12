using mapa_back.Exceptions;
using mapa_back.Models;
using mapa_back.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Numerics;
using System.Runtime.CompilerServices;

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
    }
}
