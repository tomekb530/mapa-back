using System.Numerics;
using System.Text.Json.Serialization;

namespace mapa_back.Models.DTO
{
    public class SchoolDTO
    {
        public string? Typ { get; set; }

        public string Email { get; set; }

        public string Gmina { get; set; }

        public string Nazwa { get; set; }

        public string Ulica { get; set; }

        public string Powiat { get; set; }

        public string Telefon { get; set; }

        public string Dyrektor { get; set; }

        public int RspoNumer { get; set; }

        public string? KodPocztowy { get; set; }

        public string Miejscowosc { get; set; }

        public string? NipPodmiotu { get; set; }

        public string NumerLokalu { get; set; }

        public string Wojewodztwo { get; set; }

        public string? NumerBudynku { get; set; }

        public DateTime? DataZalozenia { get; set; }

        public int? LiczbaUczniow { get; set; }

        public string? RegonPodmiotu { get; set; }

        public DateTime? DataLikwidacji { get; set; }

        public string? KategoriaUczniow { get; set; }

        public string? SpecyfikaPlacowki { get; set; }

        public string? StatusPublicznosc { get; set; }

        public string? StronaInternetowa { get; set; }

        public string? RodzajMiejscowosci { get; set; }

        public string? KodTerytorialnyGmina { get; set; }

        public string? PodmiotNadrzednyRspo { get; set; }

        public string? KodTerytorialnyPowiat { get; set; }

        public string? OrganProwadzacyPowiat { get; set; }

        public string? KodTerytorialnyMiejscowosc { get; set; }

        public string? KodTerytorialnyWojewodztwo { get; set; }

        public DateTime? DataRozpoczeciaDzialalnosci { get; set; }

        public int Id { get; set; }
        public GeographyDTO Geography { get; set; }
    }
}
