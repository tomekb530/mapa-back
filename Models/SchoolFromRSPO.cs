using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace mapa_back.Models
{
    public class SchoolFromRSPO
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("rsponumer")]
        public int RspoNumber { get; set; }

        [Column("geography")]
        public Point Geography { get; set; }

		[Column("typ")]
		public string Typ { get; set; }

		[Column("statuspublicznosc")]
		public string StatusPublicznosc { get; set; }

		[Column("nazwa")]
		public string Nazwa { get; set; }

		[Column("wojewodztwo")]
		public string Wojewodztwo { get; set; }

		[Column("kodterytorialnywojewodztwo")]
		public string KodTerytorialnyWojewodztwo { get; set; }

		[Column("gmina")]
		public string Gmina { get; set; }

		[Column("kodterytorialnygmina")]
		public string KodTerytorialnyGmina { get; set; }

		[Column("powiat")]
		public string Powiat { get; set; }

		[Column("kodterytorialnypowiat")]
		public string KodTerytorialnyPowiat { get; set; }

		[Column("organprowadzacypowiat")]
		public string OrganProwadzacyPowiat { get; set; }

		[Column("miejscowosc")]
		public string Miejscowosc { get; set; }

		[Column("rodzajmiejscowosci")]
		public string RodzajMiejscowosci { get; set; }

		[Column("kodterytorialnymiejscowosc")]
		public string KodTerytorialnyMiejscowosc { get; set; }

		[Column("kodpocztowy")]
		public string KodPocztowy { get; set; }

		[Column("ulica")]
		public string Ulica { get; set; }

		[Column("numerbudynku")]
		public string NumerBudynku { get; set; }

		[Column("numerlokalu")]
		public string NumerLokalu { get; set; }

		[Column("email")]
		public string Email { get; set; }

		[Column("telefon")]
		public string Telefon { get; set; }

		[Column("stronainternetowa")]
		public string StronaInternetowa { get; set; }

		[Column("dyrektor")]
		public string Dyrektor { get; set; }

		[Column("podmiotnadrzednyrspo")]
		public string PodmiotNadrzednyRSPO { get; set; }

		[Column("nippodmiotu")]
		public string NipPodmiotu { get; set; }

		[Column("regonpodmiotu")]
		public string RegonPodmiotu { get; set; }

		[Column("datarozpoczeciadzialalnosci")]
		public DateTime? DataRozpoczeciaDzialalnosci { get; set; }

		[Column("datazalozenia")]
		public DateTime DataZalozenia { get; set; }

		[Column("datalikwidacji	")]
		public DateTime? DataLikwidacji { get; set; }

		[Column("liczbauczniow")]
		public int? LiczbaUczniow { get; set; }

		[Column("kategoriauczniow")]
		public string KategoriaUczniow { get; set; }

		[Column("specyfikaplacowki")]
		public string SpecyfikaPlacowki { get; set; }

		[Column("typpodmiotprowadzacy")]
		public List<string> PodmiotProwadzacy { get; set; }
	}
}
