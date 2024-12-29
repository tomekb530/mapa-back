using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace mapa_back.Models.RSPOApi
{
    public class ManagingEntity
    {
        public ManagingEntity(int? id, string? nazwa, RSPOTypeSchema? typ, string? regon)
        {
            Id = id;
            Nazwa = nazwa;
            Typ = typ;
            Regon = regon;
        }

        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("nazwa")]
        public string? Nazwa { get; set; }
        [JsonPropertyName("typ")]
        public RSPOTypeSchema? Typ { get; set; }
        [JsonPropertyName("regon")]
        public string? Regon {  get; set; }
    }
}
