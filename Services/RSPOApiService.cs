using AutoMapper;
using mapa_back.Exceptions;
using mapa_back.Mappers;
using mapa_back.Models;
using mapa_back.Models.DTO;
using mapa_back.Models.RSPOApi;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace mapa_back.Services
{
    public class RSPOApiService : IRSPOApiService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;

        public RSPOApiService(DatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        

        private int GetNumberOfPages(string responseBody)
        {
            try
            {
                int numberOfPages = 0;
                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    Regex rx = new Regex("\\d+");
                    JsonElement root = doc.RootElement;
                    var regexMatch = rx.Match(root.GetProperty("hydra:view").GetProperty("hydra:last").ToString());
                    if (regexMatch.Success)
                    {
                        numberOfPages = Int32.Parse(regexMatch.Value);
                    }
                    else
                    {
                        throw new Exception("Couldn't find total pages number");
                    }
                }
                return numberOfPages;
            }
            catch (Exception ex)
            {
                throw new RSPOToDatabaseException("Unexpected error occurred while trying to get number of pages from response json");
            }
        }
        static List<SchoolApi> GetSchoolsFromResponse(string responseBody)
        {
            try
            {
                List<SchoolApi> placowki = new List<SchoolApi>();
                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    JsonElement root = doc.RootElement;
                    JsonElement schoolListJson = root.GetProperty("hydra:member");
                    placowki = JsonSerializer.Deserialize<List<SchoolApi>>(schoolListJson);
                }
                return placowki;
            }
            catch(Exception ex)
            {
                throw new RSPOToDatabaseException("Unexpected error occurred while trying to GetSchoolsFromResponse JSON");
            }
        }
        private async Task SaveSingleSchoolToDatabase(Point geography, string businessDataJson, int rspoNumber)
        {
            try
            {
                School schoolInDatabase = await _dbContext.Schools.FirstOrDefaultAsync(element => element.RspoNumber == rspoNumber);
                if(schoolInDatabase!=null)
                { 
                    schoolInDatabase.Geography = geography;
                    schoolInDatabase.BusinessData = businessDataJson;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    School newSchool = new School
                    {
                        BusinessData = businessDataJson,
                        Geography = geography,
                        RspoNumber = rspoNumber
                    };
                    _dbContext.Add(newSchool);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                throw new RSPOToDatabaseException("Unexpected error occurred while trying to save single school data to database");
            }
        }
        private async Task SaveSchoolsToDatabase(List<SchoolApi> schools)
        {
            foreach (var school in schools)
            {
                Point point;
                string businessDataJson;
                try
                {
                    BusinessDataDTO businessData = MapToBusinessData(school);
                    point = new Point(new Coordinate { X = school.Geolokalizacja.Longitude, Y = school.Geolokalizacja.Latitude });
                    businessDataJson = JsonSerializer.Serialize(businessData);
                }
                catch(Exception ex)
                {
                    throw new RSPOToDatabaseException("Unexpected error occurred while trying to Map API Data to JSON BusinessData");
                }
                await SaveSingleSchoolToDatabase(point, businessDataJson, school.RspoNumer);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        private BusinessDataDTO MapToBusinessData(SchoolApi schoolApi)
        {
            return _mapper.Map<BusinessDataDTO>(schoolApi);
        }
        public async Task<bool> SyncDataFromRSPOApi()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            string responseBody;
            string url = "https://api-rspo.mein.gov.pl/api/placowki/?page=1";
            int numberOfPages = 0;
            try
            {
                response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new RSPOToDatabaseException("Unexpected error occurred while trying to get data from RSPO API");
            }

            numberOfPages = GetNumberOfPages(responseBody);
            List<SchoolApi> schools = GetSchoolsFromResponse(responseBody);
            await SaveSchoolsToDatabase(schools);
            for (int i = 2; i < numberOfPages; i++)
            {
                try
                {
                    url = $"https://api-rspo.mein.gov.pl/api/placowki/?page={numberOfPages}";
                    response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch(Exception ex)
                {
                    throw new RSPOToDatabaseException("Unexpected error occurred while trying to get data from RSPO API");
                }
                schools = GetSchoolsFromResponse(responseBody);
                await SaveSchoolsToDatabase(schools);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return true;
            
        }
        
    }
}
