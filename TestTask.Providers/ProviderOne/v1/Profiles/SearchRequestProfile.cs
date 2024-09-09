using AutoMapper;
using TestTask.Domain.Contracts.v1.Dtos;
using TestTask.Domain.Contracts.v1.Requests;
using TestTask.Providers.ProviderOne.v1.Requests;


namespace TestTask.Providers.ProviderOne.v1.Profiles
{
    public class SearchRequestProfile : Profile
    {
        public SearchRequestProfile()
        {
            CreateMap<SearchRequest, ProviderOneSearchRequest>()
                .ForMember(dst => dst.From, opt => opt.MapFrom(src => src.Origin))
                .ForMember(dst => dst.To, opt => opt.MapFrom(src => src.Destination))
                .ForMember(dst => dst.DateFrom, opt => opt.MapFrom(src => src.OriginDateTime))
                .ForMember(dst => dst.DateTo, opt => opt.MapFrom(src => GetDateTo(src.Filters)))
                .ForMember(dst => dst.MaxPrice, opt => opt.MapFrom(src => GetMaxPrice(src.Filters)));
        }

        private DateTime? GetDateTo(SearchFilters? searchFilters)
        {
            return searchFilters?.DestinationDateTime;
        }

        private decimal? GetMaxPrice(SearchFilters? searchFilters)
        {
            return searchFilters?.MaxPrice;
        }
    }
}
