using System.Text.Json.Serialization;

namespace mapa_back.Data.RSPOApi.PodmiotProwadzacy
{
	public class PodmiotProwadzacy
	{
		[JsonPropertyName("id")]
		public int? Id { get; set; }
		[JsonPropertyName("nazwa")]
		public string? Nazwa { get; set; }

		[JsonPropertyName("typ")]
		public PodmiotProwadzacyTyp Typ { get; set; }
	}
}
