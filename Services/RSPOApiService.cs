using AutoMapper;
using mapa_back.Exceptions;
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
        private readonly HttpClient _httpClient;
        public RSPOApiService(DatabaseContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
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
        private async Task SaveSingleSchoolToDatabase(Point geography, SchoolApi schoolFromApi)
        {
            try
            {
                SchoolFromRSPO school;
                bool isNewSchool;
                isNewSchool = !await _dbContext.SchoolsFromRSPO.AnyAsync(element => element.RspoNumber == schoolFromApi.RspoNumer);
                if(isNewSchool)
                {
					school = new SchoolFromRSPO
                    {
                        RspoNumber = schoolFromApi.RspoNumer
                    };
					_dbContext.SchoolsFromRSPO.Add(school);
				}
                else
                {
					school = await _dbContext.SchoolsFromRSPO.FirstOrDefaultAsync(element => element.RspoNumber == schoolFromApi.RspoNumer);
                }
				school.Geography = geography;
				school.Typ = schoolFromApi.Typ.Nazwa;
				school.StatusPublicznosc = schoolFromApi.StatusPublicznosc.Nazwa;
				school.Nazwa = schoolFromApi.Nazwa;
				school.Wojewodztwo = schoolFromApi.Wojewodztwo;
				school.KodTerytorialnyWojewodztwo = schoolFromApi.KodTerytorialnyWojewodztwo;
				school.Gmina = schoolFromApi.Gmina;
				school.KodTerytorialnyGmina = schoolFromApi.KodTerytorialnyGmina;
				school.Powiat = schoolFromApi.Powiat;
				school.KodTerytorialnyPowiat = schoolFromApi.KodTerytorialnyPowiat;
				school.OrganProwadzacyPowiat = schoolFromApi.OrganProwadzacyPowiat;
				school.Miejscowosc = schoolFromApi.Miejscowosc;
				school.RodzajMiejscowosci = schoolFromApi.RodzajMiejscowosci;
				school.KodTerytorialnyMiejscowosc = schoolFromApi.KodTerytorialnyMiejscowosc;
				school.KodPocztowy = schoolFromApi.KodPocztowy;
				school.Ulica = schoolFromApi.Ulica;
				school.NumerBudynku = schoolFromApi.NumerBudynku;
				school.NumerLokalu = schoolFromApi.NumerLokalu;
				school.Email = schoolFromApi.Email;
				school.Telefon = schoolFromApi.Telefon;
				school.StronaInternetowa = schoolFromApi.StronaInternetowa;
				school.Dyrektor = schoolFromApi.Dyrektor;
				school.PodmiotNadrzednyRSPO = schoolFromApi.PodmiotNadrzednyRspo;
				school.NipPodmiotu = schoolFromApi.NipPodmiotu;
				school.RegonPodmiotu = schoolFromApi.RegonPodmiotu;
				school.DataRozpoczeciaDzialalnosci = schoolFromApi.DataRozpoczeciaDzialalnosci;
				school.DataZalozenia = schoolFromApi.DataZalozenia;
				school.DataLikwidacji = schoolFromApi.DataLikwidacji;
				school.LiczbaUczniow = schoolFromApi.LiczbaUczniow;
				school.KategoriaUczniow = schoolFromApi.KategoriaUczniow.Nazwa;
				school.SpecyfikaPlacowki = schoolFromApi.SpecyfikaPlacowki;
                school.PodmiotProwadzacy.AddRange(schoolFromApi.PodmiotProwadzacy.Select(x => x.Nazwa).ToList());
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
                Point point = new Point(new Coordinate { X = school.Geolokalizacja.Longitude, Y = school.Geolokalizacja.Latitude });
                await SaveSingleSchoolToDatabase(point, school);
            }
            await _dbContext.SaveChangesAsync();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public async Task SyncDataFromRSPOApi()
        {
            string url = "https://api-rspo.men.gov.pl/api/placowki/?page=1";
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
                throw new RSPOToDatabaseException("Unexpected error occurred while trying to get data from RSPO API. Check if RSPO Api changed URL");
            }
            for (int i = 1; i <= numberOfPages; i++)
            {
                try
                {
                    url = $"https://api-rspo.men.gov.pl/api/placowki/?page={i}";
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
