using System.Text.Json.Serialization;

namespace mapa_back.Models.RSPOApi
{
    public class KategoriaUczniow
    {
        [JsonPropertyName("@id")]
        public string IdPath { get; set; }
        [JsonPropertyName("@type")]
        public string Type { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nazwa")]
        public string Nazwa { get; set; }
    }
}
