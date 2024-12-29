﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace mapa_back.Models
{
    public class SchoolFromRSPO
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Column("numer_rspo")]
        [Required]
        public int NumerRspo { get; set; }

        [Column("geography")]
        [Required]
        public Point Geography { get; set; }

		[Column("typ")]
        [MaxLength(128)]
        public string? Typ { get; set; }

		[Column("status_publiczno_prawny")]
        [MaxLength(128)]
        public string? StatusPublicznoPrawny { get; set; }

		[Column("nazwa")]
        [MaxLength(256)]
        [Required]
        public string Nazwa { get; set; }

		[Column("wojewodztwo")]
        [MaxLength(32)]
        public string? Wojewodztwo { get; set; }

		[Column("gmina")]
        [MaxLength(64)]
        public string? Gmina { get; set; }

		[Column("powiat")]
        [MaxLength(32)]
        public string? Powiat { get; set; }

		[Column("miejscowosc")]
        [MaxLength(64)]
        public string? Miejscowosc { get; set; }

		[Column("gmina_rodzaj")]
        [MaxLength(32)]
        public string? GminaRodzaj { get; set; }

		[Column("kod_pocztowy")]
        [MaxLength(16)]
        public string? KodPocztowy { get; set; }

		[Column("ulica")]
        [MaxLength(128)]
        public string? Ulica { get; set; }

		[Column("numer_budynku")]
        [MaxLength(32)]
        public string? NumerBudynku { get; set; }

		[Column("numer_lokalu")]
        [MaxLength(32)]
        public string? NumerLokalu { get; set; }

		[Column("email")]
        [MaxLength(128)]
        public string? Email { get; set; }

		[Column("telefon")]
        [MaxLength(16)]
        public string? Telefon { get; set; }

		[Column("strona_internetowa")]
        [MaxLength(256)]
        public string? StronaInternetowa { get; set; }

		[Column("dyrektor_imie")]
        [MaxLength(32)]
        public string? DyrektorImie { get; set; }

        [Column("dyrektor_nazwisko")]
        [MaxLength(32)]
        public string? DyrektorNazwisko { get; set; }

		[Column("nip")]
        [MaxLength(10)]
        public string? Nip { get; set; }

		[Column("regon")]
        [MaxLength(14)]
        public string? Regon { get; set; }

		[Column("data_rozpoczecia")]
		public DateTime? DataRozpoczecia { get; set; }

		[Column("data_zalozenia")]
		public DateTime? DataZalozenia { get; set; }

        [Column("data_zakonczenia")]
        public DateTime? DataZakonczenia { get; set; }

        [Column("data_likwidacji")]
		public DateTime? DataLikwidacji { get; set; }

		[Column("liczba_uczniow")]
		public int? LiczbaUczniow { get; set; }

		[Column("kategoria_uczniow")]
        [MaxLength(64)]
        public string? KategoriaUczniow { get; set; }

		[Column("specyfika_szkoly")]
        [MaxLength(64)]
        public string? SpecyfikaSzkoly { get; set; }

		[Column("podmiot_prowadzacy")]
		public List<ManagingEntity> PodmiotProwadzacy { get; set; }
	}
}
