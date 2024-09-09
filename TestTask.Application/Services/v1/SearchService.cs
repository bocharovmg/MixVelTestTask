using TestTask.Domain.Services.v1;
using TestTask.Domain.Contracts.v1.Requests;
using TestTask.Domain.Contracts.v1.Responses;
using TestTask.Domain.Contracts.v1.Dtos;
using Microsoft.Extensions.Logging;


namespace TestTask.Application.Services.v1
{
    /// <summary>
    /// Represents a search service. The search is carried out using the third party search provider services
    /// </summary>
    public class SearchService : ISearchService
    {
        private readonly ILogger<SearchService> _logger;

        private readonly ISearchCacheService _searchCacheService;

        private readonly IEnumerable<ISearchService> _searchProviderServices;

        private readonly object _localLockObject = new object();

        public SearchService(
            ILogger<SearchService> logger,
            ISearchCacheService searchCacheService,
            IEnumerable<ISearchService> searchProviderServices
        )
        {
            _logger = logger;

            _searchCacheService = searchCacheService;

            _searchProviderServices = searchProviderServices;
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            var routes = new HashSet<Route>();

            //var routes = new List<Route>();

            decimal minPrice = 0;

            decimal maxPrice = 0;

            int minMinutesRoute = 0;

            int maxMinutesRoute = 0;

            var setMinMaxResult = (SearchResponse searchResponse) =>
            {
                minPrice = minPrice == 0 || minPrice > searchResponse.MinPrice ? searchResponse.MinPrice : minPrice;

                maxPrice = maxPrice == 0 || maxPrice < searchResponse.MaxPrice ? searchResponse.MaxPrice : maxPrice;

                minMinutesRoute = minMinutesRoute == 0 || minMinutesRoute > searchResponse.MinMinutesRoute ? searchResponse.MinMinutesRoute : minMinutesRoute;

                maxMinutesRoute = maxMinutesRoute == 0 || maxMinutesRoute < searchResponse.MaxMinutesRoute ? searchResponse.MaxMinutesRoute : maxMinutesRoute;
            };

            if (!(request.Filters?.OnlyCached ?? false))
            {
                await Parallel.ForEachAsync(
                    _searchProviderServices,
                    cancellationToken,
                    async (searchProviderService, cancellationToken) =>
                    {
                        var searchResult = await searchProviderService.SearchAsync(request, cancellationToken);

                        if (!searchResult.Routes.Any())
                        {
                            return;
                        }

                        lock (_localLockObject)
                        {
                            foreach (var route in searchResult.Routes)
                            {
                                routes.Add(route);
                            }
                            //routes.AddRange(searchResult.Routes);

                            setMinMaxResult(searchResult);
                        }
                    }
                );
            }

            var searchCacheResult = await _searchCacheService.SearchAsync(request, cancellationToken);

            await _searchCacheService.AddAsync(routes, cancellationToken);

            foreach (var route in searchCacheResult.Routes)
            {
                if (routes.Contains(route))
                {
                    continue;
                }

                routes.Add(route);

                setMinMaxResult(searchCacheResult);
            }

            var result = new SearchResponse
            {
                Routes = routes.ToArray(),
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinMinutesRoute = minMinutesRoute,
                MaxMinutesRoute = maxMinutesRoute
            };

            return result;
        }

        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            var asyncPingResults = _searchProviderServices
                .AsParallel()
                .Select(async searchProviderService => {
                    var result = await searchProviderService.IsAvailableAsync(cancellationToken);

                    return result;
                })
                .ToArray();

            var pingResults = await Task.WhenAll(asyncPingResults);

            var result = pingResults.Any(pingResult => pingResult);

            return result;
        }
    }
}
