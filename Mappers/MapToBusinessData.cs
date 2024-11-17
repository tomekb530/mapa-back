using AutoMapper;
using mapa_back.Models.DTO;
using mapa_back.Models.RSPOApi;
using NetTopologySuite.Geometries;

namespace mapa_back.Mappers
{
    public class MapToBusinessData : Profile
    {
        public MapToBusinessData()
        {         
            CreateMap<SchoolApi, BusinessDataDTO>()
                .ForMember(dest => dest.Typ, opt => opt.MapFrom(src => src.Typ.Nazwa))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Gmina, opt => opt.MapFrom(src => src.Gmina))
                .ForMember(dest => dest.Nazwa, opt => opt.MapFrom(src => src.Nazwa))
                .ForMember(dest => dest.Ulica, opt => opt.MapFrom(src => src.Ulica))
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
                .ForMember(dest => dest.KategoriaUczniow, opt => opt.MapFrom(src => src.KategoriaUczniow.Nazwa))
                .ForMember(dest => dest.SpecyfikaPlacowki, opt => opt.MapFrom(src => src.SpecyfikaPlacowki))
                .ForMember(dest => dest.StatusPublicznosc, opt => opt.MapFrom(src => src.StatusPublicznosc.Nazwa))
                .ForMember(dest => dest.StronaInternetowa, opt => opt.MapFrom(src => src.StronaInternetowa))
                .ForMember(dest => dest.RodzajMiejscowosci, opt => opt.MapFrom(src => src.RodzajMiejscowosci))
                .ForMember(dest => dest.KodTerytorialnyGmina, opt => opt.MapFrom(src => src.KodTerytorialnyGmina))
                .ForMember(dest => dest.PodmiotNadrzednyRspo, opt => opt.MapFrom(src => src.PodmiotNadrzednyRspo))
                .ForMember(dest => dest.KodTerytorialnyPowiat, opt => opt.MapFrom(src => src.KodTerytorialnyPowiat))
                .ForMember(dest => dest.OrganProwadzacyPowiat, opt => opt.MapFrom(src => src.OrganProwadzacyPowiat))
                .ForMember(dest => dest.KodTerytorialnyMiejscowosc, opt => opt.MapFrom(src => src.KodTerytorialnyMiejscowosc))
                .ForMember(dest => dest.KodTerytorialnyWojewodztwo, opt => opt.MapFrom(src => src.KodTerytorialnyWojewodztwo))
                .ForMember(dest => dest.DataRozpoczeciaDzialalnosci, opt => opt.MapFrom(src => src.DataRozpoczeciaDzialalnosci));

            CreateMap<Point, GeographyDTO>()
            .ForMember(dest => dest.X, opt => opt.MapFrom(src => src.X))
            .ForMember(dest => dest.Y, opt => opt.MapFrom(src => src.Y));

            CreateMap<BusinessDataDTO, SchoolDTO>()
                .ForMember(dest => dest.Typ, opt => opt.MapFrom(src => src.Typ))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Gmina, opt => opt.MapFrom(src => src.Gmina))
                .ForMember(dest => dest.Nazwa, opt => opt.MapFrom(src => src.Nazwa))
                .ForMember(dest => dest.Ulica, opt => opt.MapFrom(src => src.Ulica))
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
                .ForMember(dest => dest.KategoriaUczniow, opt => opt.MapFrom(src => src.KategoriaUczniow))
                .ForMember(dest => dest.SpecyfikaPlacowki, opt => opt.MapFrom(src => src.SpecyfikaPlacowki))
                .ForMember(dest => dest.StatusPublicznosc, opt => opt.MapFrom(src => src.StatusPublicznosc))
                .ForMember(dest => dest.StronaInternetowa, opt => opt.MapFrom(src => src.StronaInternetowa))
                .ForMember(dest => dest.RodzajMiejscowosci, opt => opt.MapFrom(src => src.RodzajMiejscowosci))
                .ForMember(dest => dest.KodTerytorialnyGmina, opt => opt.MapFrom(src => src.KodTerytorialnyGmina))
                .ForMember(dest => dest.PodmiotNadrzednyRspo, opt => opt.MapFrom(src => src.PodmiotNadrzednyRspo))
                .ForMember(dest => dest.KodTerytorialnyPowiat, opt => opt.MapFrom(src => src.KodTerytorialnyPowiat))
                .ForMember(dest => dest.OrganProwadzacyPowiat, opt => opt.MapFrom(src => src.OrganProwadzacyPowiat))
                .ForMember(dest => dest.KodTerytorialnyMiejscowosc, opt => opt.MapFrom(src => src.KodTerytorialnyMiejscowosc))
                .ForMember(dest => dest.KodTerytorialnyWojewodztwo, opt => opt.MapFrom(src => src.KodTerytorialnyWojewodztwo))
                .ForMember(dest => dest.DataRozpoczeciaDzialalnosci, opt => opt.MapFrom(src => src.DataRozpoczeciaDzialalnosci))
                .ForMember(dest => dest.Id, opt => opt.MapFrom((src, dest, _, context) =>
                 context.Items["id"]))
                .ForMember(dest => dest.Geography, opt => opt.MapFrom((src, dest, _, context) =>
                 context.Items["geography"]));
        }
    }
}
