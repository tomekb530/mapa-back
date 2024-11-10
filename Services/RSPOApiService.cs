using System.Text.Json;
using System.Text.RegularExpressions;

namespace mapa_back.Services
{
    public class RSPOApiService : IRSPOApiService
    {
        private readonly DatabaseContext _dbContext;

        public RSPOApiService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        

        private int GetNumberOfPages(string responseBody)
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

        //TO DO
        //Dodac aktualizacje naszej bazy danych (jak Patryk mi pokaze jak json u nas wyglada i biznes data) 
        public async Task<bool> SyncDataFromRSPOApi()
        {
            HttpClient client = new HttpClient();
            string url = "https://api-rspo.mein.gov.pl/api/placowki/?page=1";
            int numberOfPages = 0;
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                numberOfPages = GetNumberOfPages(responseBody);
                for (int i = 2; i < numberOfPages; i++)
                {
                    url = $"https://api-rspo.mein.gov.pl/api/placowki/?page={numberOfPages}";
                    response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                    //pacowki.AddRange(GetPlacowkiFromResponse(responseBody));
                }
                return true;
            }
            catch (Exception ex)
            {
                //zrobic potem kurwa lapanie roznych wyjatkow
                throw new Exception(ex.Message);
            }
        }
        
    }
}
