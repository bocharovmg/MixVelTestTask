using Microsoft.Extensions.DependencyInjection;
using TestTask.Domain.Contracts.v1.Dtos;
using TestTask.Domain.Contracts.v1.Requests;
using TestTask.Domain.Services.v1;


namespace TestTask.Application.UnitTests.SearchService
{
    public class SearchServiceTest : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public SearchServiceTest(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData(
            nameof(SearchServiceTestScenarios.Search_OriginDateTime_RandomScenarioDatas),
            parameters: [50],
            MemberType = typeof(SearchServiceTestScenarios)
        )]
        public async Task SearchAsync_OriginDateTime_CacheTest(Route[] routes, SearchRequest request, int expectedResultCount)
        {
            var cacheService = _fixture.ServiceProvider.GetRequiredService<ISearchCacheService>();

            var searchService = _fixture.ServiceProvider.GetRequiredService<ISearchService>();

            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(20000);

            await cacheService.AddAsync(routes, cancellationTokenSource.Token);

            var searchResponse = await searchService.SearchAsync(request, cancellationTokenSource.Token);

            Assert.Equal(expectedResultCount, searchResponse.Routes.Length);
        }

        [Theory]
        [MemberData(
            nameof(SearchServiceTestScenarios.Search_DestinationDateTime_RandomScenarioDatas),
            parameters: [50],
            MemberType = typeof(SearchServiceTestScenarios)
        )]
        public async Task SearchAsync_DestinationDateTime_CacheTest(Route[] routes, SearchRequest request, int expectedResultCount)
        {
            var cacheService = _fixture.ServiceProvider.GetRequiredService<ISearchCacheService>();

            var searchService = _fixture.ServiceProvider.GetRequiredService<ISearchService>();

            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(20000);

            await cacheService.AddAsync(routes, cancellationTokenSource.Token);

            var searchResponse = await searchService.SearchAsync(request, cancellationTokenSource.Token);

            Assert.Equal(expectedResultCount, searchResponse.Routes.Length);
        }

        [Theory]
        [MemberData(
            nameof(SearchServiceTestScenarios.Search_Price_RandomScenarioDatas),
            parameters: [50],
            MemberType = typeof(SearchServiceTestScenarios)
        )]
        public async Task SearchAsync_Price_CacheTest(Route[] routes, SearchRequest request, int expectedResultCount)
        {
            var cacheService = _fixture.ServiceProvider.GetRequiredService<ISearchCacheService>();

            var searchService = _fixture.ServiceProvider.GetRequiredService<ISearchService>();

            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(20000);

            await cacheService.AddAsync(routes, cancellationTokenSource.Token);

            var searchResponse = await searchService.SearchAsync(request, cancellationTokenSource.Token);

            Assert.Equal(expectedResultCount, searchResponse.Routes.Length);
        }

        [Theory]
        [MemberData(
            nameof(SearchServiceTestScenarios.Search_MinTimeLimit_RandomScenarioDatas),
            parameters: [50],
            MemberType = typeof(SearchServiceTestScenarios)
        )]
        public async Task SearchAsync_MinTimeLimit_CacheTest(Route[] routes, SearchRequest request, int expectedResultCount)
        {
            var cacheService = _fixture.ServiceProvider.GetRequiredService<ISearchCacheService>();

            var searchService = _fixture.ServiceProvider.GetRequiredService<ISearchService>();

            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(20000);

            await cacheService.AddAsync(routes, cancellationTokenSource.Token);

            var searchResponse = await searchService.SearchAsync(request, cancellationTokenSource.Token);

            Assert.Equal(expectedResultCount, searchResponse.Routes.Length);
        }
    }
}
