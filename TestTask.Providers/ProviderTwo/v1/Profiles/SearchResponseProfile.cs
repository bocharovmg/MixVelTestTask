using AutoMapper;
using TestTask.Domain.Contracts.v1.Dtos;
using TestTask.Domain.Contracts.v1.Responses;
using TestTask.Providers.ProviderTwo.v1.Dtos;
using TestTask.Providers.ProviderTwo.v1.Responses;


namespace TestTask.Providers.ProviderTwo.v1.Profiles
{
    public class SearchResponseProfile : Profile
    {
        public SearchResponseProfile()
        {
            CreateMap<ProviderTwoSearchResponse, SearchResponse>()
                .ForMember(dst => dst.MinPrice, opt => opt.MapFrom(src => GetMinPrice(src.Routes)))
                .ForMember(dst => dst.MaxPrice, opt => opt.MapFrom(src => GetMaxPrice(src.Routes)))
                .ForMember(dst => dst.MinMinutesRoute, opt => opt.MapFrom(src => GetMinMinutesRoute(src.Routes)))
                .ForMember(dst => dst.MaxMinutesRoute, opt => opt.MapFrom(src => GetMaxMinutesRoute(src.Routes)));

            CreateMap<ProviderTwoRoute, Route>()
                .ForMember(dst => dst.Origin, opt => opt.MapFrom(src => src.Departure.Point))
                .ForMember(dst => dst.Destination, opt => opt.MapFrom(src => src.Arrival.Point))
                .ForMember(dst => dst.OriginDateTime, opt => opt.MapFrom(src => src.Departure.Date))
                .ForMember(dst => dst.DestinationDateTime, opt => opt.MapFrom(src => src.Arrival.Date))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dst => dst.TimeLimit, opt => opt.MapFrom(src => src.TimeLimit));
        }

        private decimal GetMinPrice(IEnumerable<ProviderTwoRoute> routes)
        {
            return routes.Min(route => route.Price);
        }

        private decimal GetMaxPrice(IEnumerable<ProviderTwoRoute> routes)
        {
            return routes.Max(route => route.Price);
        }

        private double GetMinMinutesRoute(IEnumerable<ProviderTwoRoute> routes)
        {
            return routes.Min(route => route.Arrival.Date.Subtract(route.Departure.Date).TotalMinutes);
        }

        private double GetMaxMinutesRoute(IEnumerable<ProviderTwoRoute> routes)
        {
            return routes.Max(route => route.Arrival.Date.Subtract(route.Departure.Date).TotalMinutes);
        }
    }
}
