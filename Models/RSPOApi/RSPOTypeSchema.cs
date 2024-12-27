﻿using System.Text.Json.Serialization;

namespace mapa_back.Models.RSPOApi
{
    public class RSPOTypeSchema
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("nazwa")]
        public string? Nazwa { get; set; }
    }
}
