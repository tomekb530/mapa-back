using AutoMapper;
using mapa_back.Models.DTO;
using mapa_back.Models.RSPOApi;

namespace mapa_back.Mappers
{
    public class MapToBusinessData : Profile
    {
        public MapToBusinessData()
        {
            CreateMap<SchoolApi, BusinessDataDTO>()
                .ForMember(dest => dest.Typ, opt => opt.MapFrom(src => src.Typ.Nazwa))
                .ForMember(dest => dest.Faks, opt => opt.MapFrom(src => src.Faks))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Gmina, opt => opt.MapFrom(src => src.Gmina))
                .ForMember(dest => dest.Nazwa, opt => opt.MapFrom(src => src.Nazwa))
                .ForMember(dest => dest.Ulica, opt => opt.MapFrom(src => src.Ulica))
                .ForMember(dest => dest.Poczta, opt => opt.MapFrom(src => src.Poczta))
                .ForMember(dest => dest.Powiat, opt => opt.MapFrom(src => src.Powiat))
                .ForMember(dest => dest.Telefon, opt => opt.MapFrom(src => src.Telefon))
                .ForMember(dest => dest.Dyrektor, opt => opt.MapFrom(src => src.Dyrektor))
                .ForMember(dest => dest.RspoNumer, opt => opt.MapFrom(src => src.RspoNumer))
                .ForMember(dest => dest.KodPocztowy, opt => opt.MapFrom(src => src.KodPocztowy))
                .ForMember(dest => dest.Miejscowosc, opt => opt.MapFrom(src => src.Miejscowosc))
                .ForMember(dest => dest.NipPodmiotu, opt => opt.MapFrom(src => src.NipPodmiotu))
                .ForMember(dest => dest.NumerLokalu, opt => opt.MapFrom(src => src.NumerLokalu))
                .ForMember(dest => dest.Wojewodztwo, opt => opt.MapFrom(src => src.Wojewodztwo))
                .ForMember(dest => dest.NumerBudynku, opt => opt.MapFrom(src => src.NumerBudynku))
                .ForMember(dest => dest.DataZalozenia, opt => opt.MapFrom(src => src.DataZalozenia))
                .ForMember(dest => dest.LiczbaUczniow, opt => opt.MapFrom(src => src.LiczbaUczniow))
                .ForMember(dest => dest.RegonPodmiotu, opt => opt.MapFrom(src => src.RegonPodmiotu))
                .ForMember(dest => dest.DataLikwidacji, opt => opt.MapFrom(src => src.DataLikwidacji))
                .ForMember(dest => dest.JezykiNauczane, opt => opt.MapFrom(src => src.JezykiNauczane))
                .ForMember(dest => dest.TerenySportowe, opt => opt.MapFrom(src => src.TerenySportowe))
                .ForMember(dest => dest.KategoriaUczniow, opt => opt.MapFrom(src => src.KategoriaUczniow.Nazwa))
                .ForMember(dest => dest.StrukturaMiejsce, opt => opt.MapFrom(src => src.StrukturaMiejsce))
                .ForMember(dest => dest.SpecyfikaPlacowki, opt => opt.MapFrom(src => src.SpecyfikaPlacowki))
                .ForMember(dest => dest.StatusPublicznosc, opt => opt.MapFrom(src => src.StatusPublicznosc.Nazwa))
                .ForMember(dest => dest.StronaInternetowa, opt => opt.MapFrom(src => src.StronaInternetowa))
                .ForMember(dest => dest.OrganProwadzacyNip, opt => opt.MapFrom(src => src.OrganProwadzacyNip))
                .ForMember(dest => dest.OrganProwadzacyTyp, opt => opt.MapFrom(src => src.OrganProwadzacyTyp))
                .ForMember(dest => dest.RodzajMiejscowosci, opt => opt.MapFrom(src => src.RodzajMiejscowosci))
                .ForMember(dest => dest.PodmiotNadrzednyTyp, opt => opt.MapFrom(src => src.PodmiotNadrzednyTyp))
                .ForMember(dest => dest.KodTerytorialnyGmina, opt => opt.MapFrom(src => src.KodTerytorialnyGmina))
                .ForMember(dest => dest.OrganProwadzacyGmina, opt => opt.MapFrom(src => src.OrganProwadzacyGmina))
                .ForMember(dest => dest.OrganProwadzacyNazwa, opt => opt.MapFrom(src => src.OrganProwadzacyNazwa))
                .ForMember(dest => dest.OrganProwadzacyRegon, opt => opt.MapFrom(src => src.OrganProwadzacyRegon))
                .ForMember(dest => dest.PodmiotNadrzednyRspo, opt => opt.MapFrom(src => src.PodmiotNadrzednyRspo))
                .ForMember(dest => dest.KodTerytorialnyPowiat, opt => opt.MapFrom(src => src.KodTerytorialnyPowiat))
                .ForMember(dest => dest.OrganProwadzacyPowiat, opt => opt.MapFrom(src => src.OrganProwadzacyPowiat))
                .ForMember(dest => dest.PodmiotNadrzednyNazwa, opt => opt.MapFrom(src => src.PodmiotNadrzednyNazwa))
                .ForMember(dest => dest.KodTerytorialnyMiejscowosc, opt => opt.MapFrom(src => src.KodTerytorialnyMiejscowosc))
                .ForMember(dest => dest.KodTerytorialnyWojewodztwo, opt => opt.MapFrom(src => src.KodTerytorialnyWojewodztwo))
                .ForMember(dest => dest.OrganProwadzacyWojewodztwo, opt => opt.MapFrom(src => src.OrganProwadzacyWojewodztwo))
                .ForMember(dest => dest.DataRozpoczeciaDzialalnosci, opt => opt.MapFrom(src => src.DataRozpoczeciaDzialalnosci));
        }
    }
}
