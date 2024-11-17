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
        private readonly HttpClient _httpClient;
        public RSPOApiService(DatabaseContext dbContext, IMapper mapper, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClient;
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
            catch (Exception)
            {
                throw new RSPOToDatabaseException("Unexpected error occurred while trying to get number of pages from response json");
            }
        }
        static List<SchoolApi> GetSchoolsFromResponse(string responseBody)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    JsonElement root = doc.RootElement;
                    JsonElement schoolListJson = root.GetProperty("hydra:member");
                    List<SchoolApi> placowki = JsonSerializer.Deserialize<List<SchoolApi>>(schoolListJson) ?? new List<SchoolApi>();
                    return placowki;
                }
            }
            catch(Exception)
            {
                throw new RSPOToDatabaseException("Unexpected error occurred while trying to GetSchoolsFromResponse JSON");
            }
        }
        private async Task SaveSingleSchoolToDatabase(Point geography, string businessDataJson, int rspoNumber)
        {
            try
            {
                SchoolFromRSPO schoolInDatabase = await _dbContext.SchoolsFromRSPO.FirstOrDefaultAsync(element => element.RspoNumber == rspoNumber) ?? throw new RSPOToDatabaseException("Cannot find element in database");
                if(schoolInDatabase!=null)
                { 
                    schoolInDatabase.Geography = geography;
                    schoolInDatabase.BusinessData = businessDataJson;
                }
                else
                {
                    _dbContext.SchoolsFromRSPO.Add(new SchoolFromRSPO
                    {
                        BusinessData = businessDataJson,
                        Geography = geography,
                        RspoNumber = rspoNumber
                    });
                }
            }
            catch(Exception)
            {
                throw new RSPOToDatabaseException("Unexpected error occurred while trying to save single school data to database");
            }
        }
        private async Task SaveSchoolsToDatabase(List<SchoolApi> schools)
        {
            foreach (var school in schools)
            {
                BusinessDataDTO businessData = MapToBusinessData(school);
                Point point = new Point(new Coordinate { X = school.Geolokalizacja.Longitude, Y = school.Geolokalizacja.Latitude });
                string businessDataJson = JsonSerializer.Serialize(businessData);
                await SaveSingleSchoolToDatabase(point, businessDataJson, school.RspoNumer);
            }
            await _dbContext.SaveChangesAsync();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        private BusinessDataDTO MapToBusinessData(SchoolApi schoolApi)
        {
            return _mapper.Map<BusinessDataDTO>(schoolApi);
        }
        public async Task SyncDataFromRSPOApi()
        {
            string url = "https://api-rspo.mein.gov.pl/api/placowki/?page=1";
            int numberOfPages = 0;
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    numberOfPages = GetNumberOfPages(responseBody);
                }
            }
            catch (Exception)
            {
                throw new RSPOToDatabaseException("Unexpected error occurred while trying to get data from RSPO API");
            }
            for (int i = 1; i <= numberOfPages; i++)
            {
                try
                {
                    url = $"https://api-rspo.mein.gov.pl/api/placowki/?page={i}";
                    using (HttpResponseMessage response = await _httpClient.GetAsync(url,HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<SchoolApi> schools = GetSchoolsFromResponse(responseBody);
                        await SaveSchoolsToDatabase(schools);
                    }
                }
                catch(RSPOToDatabaseException)
                {
                    throw;
                }
                catch(Exception)
                {
                    throw new RSPOToDatabaseException("Unexpected error occurred while trying to get data from RSPO API");
                }
                Console.WriteLine($"Readed page nr {i}");
            }     
        }
        
    }
}
