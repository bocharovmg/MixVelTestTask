using AutoMapper;
using TestTask.Domain.Contracts.v1.Dtos;
using TestTask.Domain.Contracts.v1.Responses;
using TestTask.Providers.ProviderOne.v1.Dtos;
using TestTask.Providers.ProviderOne.v1.Responses;


namespace TestTask.Providers.ProviderOne.v1.Profiles
{
    public class SearchResponseProfile : Profile
    {
        public SearchResponseProfile()
        {
            CreateMap<ProviderOneSearchResponse, SearchResponse>()
                .ForMember(dst => dst.MinPrice, opt => opt.MapFrom(src => GetMinPrice(src.Routes)))
                .ForMember(dst => dst.MaxPrice, opt => opt.MapFrom(src => GetMaxPrice(src.Routes)))
                .ForMember(dst => dst.MinMinutesRoute, opt => opt.MapFrom(src => GetMinMinutesRoute(src.Routes)))
                .ForMember(dst => dst.MaxMinutesRoute, opt => opt.MapFrom(src => GetMaxMinutesRoute(src.Routes)));

            CreateMap<ProviderOneRoute, Route>()
                .ForMember(dst => dst.Origin, opt => opt.MapFrom(src => src.From))
                .ForMember(dst => dst.Destination, opt => opt.MapFrom(src => src.To))
                .ForMember(dst => dst.OriginDateTime, opt => opt.MapFrom(src => src.DateFrom))
                .ForMember(dst => dst.DestinationDateTime, opt => opt.MapFrom(src => src.DateTo))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dst => dst.TimeLimit, opt => opt.MapFrom(src => src.TimeLimit));
        }

        private decimal GetMinPrice(IEnumerable<ProviderOneRoute> routes)
        {
            return routes.Min(route => route.Price);
        }

        private decimal GetMaxPrice(IEnumerable<ProviderOneRoute> routes)
        {
            return routes.Max(route => route.Price);
        }

        private double GetMinMinutesRoute(IEnumerable<ProviderOneRoute> routes)
        {
            return routes.Min(route => route.DateTo.Subtract(route.DateFrom).TotalMinutes);
        }

        private double GetMaxMinutesRoute(IEnumerable<ProviderOneRoute> routes)
        {
            return routes.Max(route => route.DateTo.Subtract(route.DateFrom).TotalMinutes);
        }
    }
}
