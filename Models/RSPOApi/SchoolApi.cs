﻿using System.Text.Json.Serialization;

namespace mapa_back.Models.RSPOApi
{
    public class SchoolApi
    {
        [JsonPropertyName("typ")]
        public RSPOTypeSchema? Typ { get; set; }

        [JsonPropertyName("faks")]
        public string? Faks { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("gmina")]
        public string Gmina { get; set; }

        [JsonPropertyName("nazwa")]
        public string Nazwa { get; set; }

        [JsonPropertyName("ulica")]
        public string Ulica { get; set; }

        [JsonPropertyName("poczta")]
        public string? Poczta { get; set; }

        [JsonPropertyName("powiat")]
        public string Powiat { get; set; }

        [JsonPropertyName("telefon")]
        public string Telefon { get; set; }

        [JsonPropertyName("dyrektorImie")]
        public string? DyrektorImie { get; set; } 

        [JsonPropertyName("dyrektorNazwisko")]
        public string? DyrektorNazwisko { get; set; } 

        [JsonIgnore]
        public string Dyrektor => $"{DyrektorImie} {DyrektorNazwisko}".Trim(); 

        [JsonPropertyName("numerRspo")]
        public int RspoNumer { get; set; }

        [JsonPropertyName("kodPocztowy")]
        public string KodPocztowy { get; set; }

        [JsonPropertyName("miejscowosc")]
        public string Miejscowosc { get; set; }

        [JsonPropertyName("nipPodmiotu")]
        public string? NipPodmiotu { get; set; }

        [JsonPropertyName("numerLokalu")]
        public string NumerLokalu { get; set; }

        [JsonPropertyName("wojewodztwo")]
        public string Wojewodztwo { get; set; }

        [JsonPropertyName("numerBudynku")]
        public string? NumerBudynku { get; set; }

        [JsonPropertyName("dataZalozenia")]
        public DateTime DataZalozenia { get; set; }

        [JsonPropertyName("liczbaUczniow")]
        public int? LiczbaUczniow { get; set; }

        [JsonPropertyName("regonPodmiotu")]
        public string? RegonPodmiotu { get; set; }

        [JsonPropertyName("dataLikwidacji")]
        public DateTime? DataLikwidacji { get; set; }

        [JsonPropertyName("jezykiNauczane")]
        public List<string>? JezykiNauczane { get; set; }

        [JsonPropertyName("terenySportowe")]
        public string? TerenySportowe { get; set; }

        [JsonPropertyName("kategoriaUczniow")]
        public RSPOTypeSchema? KategoriaUczniow { get; set; }

        [JsonPropertyName("strukturaMiejsce")]
        public string? StrukturaMiejsce { get; set; }

        [JsonPropertyName("specyfikaPlacowki")]
        public string? SpecyfikaPlacowki { get; set; }

        [JsonPropertyName("statusPublicznoPrawny")]
        public RSPOTypeSchema? StatusPublicznosc { get; set; }

        [JsonPropertyName("stronaInternetowa")]
        public string? StronaInternetowa { get; set; }

        [JsonPropertyName("organProwadzacyNip")]
        public string? OrganProwadzacyNip { get; set; }

        [JsonPropertyName("organProwadzacyTyp")]
        public string? OrganProwadzacyTyp { get; set; }

        [JsonPropertyName("gminaRodzaj")]
        public string? RodzajMiejscowosci { get; set; }

        [JsonPropertyName("podmiotNadrzednyTyp")]
        public string? PodmiotNadrzednyTyp { get; set; }

        [JsonPropertyName("gminaKodTERYT")]
        public string? KodTerytorialnyGmina { get; set; }

        [JsonPropertyName("organProwadzacyGmina")]
        public string? OrganProwadzacyGmina { get; set; }

        [JsonPropertyName("organProwadzacyNazwa")]
        public string? OrganProwadzacyNazwa { get; set; }

        [JsonPropertyName("organProwadzacyRegon")]
        public string? OrganProwadzacyRegon { get; set; }

        [JsonPropertyName("podmiotNadrzednyRspo")]
        public string? PodmiotNadrzednyRspo { get; set; }

        [JsonPropertyName("powiatKodTERYT")]
        public string? KodTerytorialnyPowiat { get; set; }

        [JsonPropertyName("organProwadzacyPowiat")]
        public string? OrganProwadzacyPowiat { get; set; }

        [JsonPropertyName("podmiotNadrzedny")]
        public string? PodmiotNadrzednyNazwa { get; set; }

        [JsonPropertyName("miejscowoscKodTERYT")]
        public string? KodTerytorialnyMiejscowosc { get; set; }

        [JsonPropertyName("wojewodztwoKodTERYT")]
        public string? KodTerytorialnyWojewodztwo { get; set; }

        [JsonPropertyName("organProwadzacyWojewodztwo")]
        public string? OrganProwadzacyWojewodztwo { get; set; }

        [JsonPropertyName("dataRozpoczecia")]
        public DateTime? DataRozpoczeciaDzialalnosci { get; set; }

        [JsonPropertyName("geolokalizacja")]
        public Geolokalizacja Geolokalizacja { get; set; }

    }
}
