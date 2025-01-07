using System.Text.Json.Serialization;

namespace mapa_back.Models.RSPOApi
{
    public class Geolokalizacja
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}
