﻿using mapa_back.Models.DTO;
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
				NumerRspo = school.NumerRspo,
				Typ = school.Typ,
				StatusPublicznoPrawny = school.StatusPublicznoPrawny,
				Nazwa = school.Nazwa,
				Wojewodztwo = school.Wojewodztwo,
				Gmina = school.Gmina,
				Powiat = school.Powiat,
				Miejscowosc = school.Miejscowosc,
				GminaRodzaj = school.GminaRodzaj,
				KodPocztowy = school.KodPocztowy,
				Ulica = school.Ulica,
				NumerBudynku = school.NumerBudynku,
				NumerLokalu = school.NumerLokalu,
				Email = school.Email,
				Telefon = school.Telefon,
				StronaInternetowa = school.StronaInternetowa,
				DyrektorImie = school.DyrektorImie,
                DyrektorNazwisko = school.DyrektorNazwisko,
                Nip = school.Nip,
				Regon = school.Regon,
				DataRozpoczecia = school.DataRozpoczecia,
				DataZalozenia = school.DataZalozenia,
				DataZakonczenia = school.DataZakonczenia,
				DataLikwidacji = school.DataLikwidacji,
				LiczbaUczniow = school.LiczbaUczniow,
				KategoriaUczniow = school.KategoriaUczniow,
				SpecyfikaSzkoly = school.SpecyfikaSzkoly,
				PodmiotProwadzacy = school.PodmiotProwadzacy,
				Geography = MapGeographyToDTO(school.Geography)
			};
		}

		public static SchoolDTO MapToDTO(School school)
		{
			return new SchoolDTO
			{
                Id = school.Id,
                NumerRspo = school.NumerRspo,
                Typ = school.Typ,
                StatusPublicznoPrawny = school.StatusPublicznoPrawny,
                Nazwa = school.Nazwa,
                Wojewodztwo = school.Wojewodztwo,
                Gmina = school.Gmina,
                Powiat = school.Powiat,
                Miejscowosc = school.Miejscowosc,
                GminaRodzaj = school.GminaRodzaj,
                KodPocztowy = school.KodPocztowy,
                Ulica = school.Ulica,
                NumerBudynku = school.NumerBudynku,
                NumerLokalu = school.NumerLokalu,
                Email = school.Email,
                Telefon = school.Telefon,
                StronaInternetowa = school.StronaInternetowa,
                DyrektorImie = school.DyrektorImie,
                DyrektorNazwisko = school.DyrektorNazwisko,
                Nip = school.Nip,
                Regon = school.Regon,
                DataRozpoczecia = school.DataRozpoczecia,
                DataZalozenia = school.DataZalozenia,
                DataZakonczenia = school.DataZakonczenia,
                DataLikwidacji = school.DataLikwidacji,
                LiczbaUczniow = school.LiczbaUczniow,
                KategoriaUczniow = school.KategoriaUczniow,
                SpecyfikaSzkoly = school.SpecyfikaSzkoly,
                PodmiotProwadzacy = school.PodmiotProwadzacy,
                Geography = MapGeographyToDTO(school.Geography)
            };
		}

        private static GeographyDTO? MapGeographyToDTO(Point geography)
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
