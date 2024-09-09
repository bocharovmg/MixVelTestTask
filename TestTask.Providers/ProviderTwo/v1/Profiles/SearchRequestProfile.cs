using AutoMapper;
using TestTask.Domain.Contracts.v1.Dtos;
using TestTask.Domain.Contracts.v1.Requests;
using TestTask.Providers.ProviderTwo.v1.Requests;


namespace TestTask.Providers.ProviderTwo.v1.Profiles
{
    public class SearchRequestProfile : Profile
    {
        public SearchRequestProfile()
        {
            CreateMap<SearchRequest, ProviderTwoSearchRequest>()
                .ForMember(dst => dst.Departure, opt => opt.MapFrom(src => src.Origin))
                .ForMember(dst => dst.Arrival, opt => opt.MapFrom(src => src.Destination))
                .ForMember(dst => dst.DepartureDate, opt => opt.MapFrom(src => src.OriginDateTime))
                .ForMember(dst => dst.MinTimeLimit, opt => opt.MapFrom(src => GetMinTimeLimit(src.Filters)));
        }

        private DateTime? GetMinTimeLimit(SearchFilters? searchFilters)
        {
            return searchFilters?.MinTimeLimit;
        }
    }
}
