using mapa_back.Models.DTO;
using mapa_back.Models;
using NetTopologySuite.Geometries;

namespace mapa_back.Mappers
{
	public class SchoolMapper
	{
		public static SchoolDTO MapToDTO(SchoolFromRSPO school)
		{
			return new SchoolDTO
			{
				Id = school.Id,
				RspoNumber = school.RspoNumber,
				Typ = school.Typ,
				StatusPublicznosc = school.StatusPublicznosc,
				Nazwa = school.Nazwa,
				Wojewodztwo = school.Wojewodztwo,
				KodTerytorialnyWojewodztwo = school.KodTerytorialnyWojewodztwo,
				Gmina = school.Gmina,
				KodTerytorialnyGmina = school.KodTerytorialnyGmina,
				Powiat = school.Powiat,
				KodTerytorialnyPowiat = school.KodTerytorialnyPowiat,
				OrganProwadzacyPowiat = school.OrganProwadzacyPowiat,
				Miejscowosc = school.Miejscowosc,
				RodzajMiejscowosci = school.RodzajMiejscowosci,
				KodTerytorialnyMiejscowosc = school.KodTerytorialnyMiejscowosc,
				KodPocztowy = school.KodPocztowy,
				Ulica = school.Ulica,
				NumerBudynku = school.NumerBudynku,
				NumerLokalu = school.NumerLokalu,
				Email = school.Email,
				Telefon = school.Telefon,
				StronaInternetowa = school.StronaInternetowa,
				Dyrektor = school.Dyrektor,
				PodmiotNadrzednyRSPO = school.PodmiotNadrzednyRSPO,
				NipPodmiotu = school.NipPodmiotu,
				RegonPodmiotu = school.RegonPodmiotu,
				DataRozpoczeciaDzialalnosci = school.DataRozpoczeciaDzialalnosci ?? DateTime.MinValue,
				DataZalozenia = school.DataZalozenia,
				DataLikwidacji = school.DataLikwidacji,
				LiczbaUczniow = school.LiczbaUczniow ?? 0,
				KategoriaUczniow = school.KategoriaUczniow,
				SpecyfikaPlacowki = school.SpecyfikaPlacowki,
				PodmiotProwadzacy = school.PodmiotProwadzacy,
				Geography = MapGeographyToDTO(school.Geography)
			};
		}

		public static SchoolDTO MapToDTO(School school)
		{
			return new SchoolDTO
			{
				Id = school.Id,
				RspoNumber = school.RspoNumber,
				Typ = school.Typ,
				StatusPublicznosc = school.StatusPublicznosc,
				Nazwa = school.Nazwa,
				Wojewodztwo = school.Wojewodztwo,
				KodTerytorialnyWojewodztwo = school.KodTerytorialnyWojewodztwo,
				Gmina = school.Gmina,
				KodTerytorialnyGmina = school.KodTerytorialnyGmina,
				Powiat = school.Powiat,
				KodTerytorialnyPowiat = school.KodTerytorialnyPowiat,
				OrganProwadzacyPowiat = school.OrganProwadzacyPowiat,
				Miejscowosc = school.Miejscowosc,
				RodzajMiejscowosci = school.RodzajMiejscowosci,
				KodTerytorialnyMiejscowosc = school.KodTerytorialnyMiejscowosc,
				KodPocztowy = school.KodPocztowy,
				Ulica = school.Ulica,
				NumerBudynku = school.NumerBudynku,
				NumerLokalu = school.NumerLokalu,
				Email = school.Email,
				Telefon = school.Telefon,
				StronaInternetowa = school.StronaInternetowa,
				Dyrektor = school.Dyrektor,
				PodmiotNadrzednyRSPO = school.PodmiotNadrzednyRSPO,
				NipPodmiotu = school.NipPodmiotu,
				RegonPodmiotu = school.RegonPodmiotu,
				DataRozpoczeciaDzialalnosci = school.DataRozpoczeciaDzialalnosci,
				DataZalozenia = school.DataZalozenia,
				DataLikwidacji = school.DataLikwidacji,
				LiczbaUczniow = school.LiczbaUczniow,
				KategoriaUczniow = school.KategoriaUczniow,
				SpecyfikaPlacowki = school.SpecyfikaPlacowki,
				PodmiotProwadzacy = school.PodmiotProwadzacy,
				Geography = MapGeographyToDTO(school.Geography)
			};
		}

		private static GeographyDTO MapGeographyToDTO(Point geography)
		{
			if (geography == null) return null;

			return new GeographyDTO
			{
				Y = geography.Y,
				X = geography.X
			};
		}
	}
}
