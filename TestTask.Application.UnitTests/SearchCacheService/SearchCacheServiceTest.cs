using Microsoft.Extensions.Logging;
using TestTask.Domain.Contracts.v1.Dtos;
using TestTask.Domain.Contracts.v1.Requests;


namespace TestTask.Application.UnitTests.SearchCacheService
{
    public class SearchCacheServiceTest
    {
        [Theory]
        [MemberData(
            nameof(SearchCacheServiceTestScenarios.NRoutesInOneTime_ScenarioDatas),
            parameters: [10, 1000],
            MemberType = typeof(SearchCacheServiceTestScenarios)
        )]
        public async Task AddAsync_NRoutesInOneTime_Test(Route[] routes)
        {
            var factory = new LoggerFactory();

            var logger = factory.CreateLogger<Services.v1.SearchCacheService>();

            var cacheService = new Services.v1.SearchCacheService(logger);

            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(2000);

            var cacheKeys = await cacheService.AddAsync(routes, cancellationTokenSource.Token);

            Assert.Equal(routes.Length, cacheKeys.Count());
        }

        [Theory]
        [MemberData(
            nameof(SearchCacheServiceTestScenarios.NRoutesInOneTime_ScenarioDatas),
            parameters: [10, 1000],
            MemberType = typeof(SearchCacheServiceTestScenarios)
        )]
        public async Task RomoveAsync_NRoutesInOneTime_Test(Route[] routes)
        {
            var factory = new LoggerFactory();

            var logger = factory.CreateLogger<Services.v1.SearchCacheService>();

            var cacheService = new Services.v1.SearchCacheService(logger);

            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(2000);

            var cacheKeys = await cacheService.AddAsync(routes, cancellationTokenSource.Token);

            var removedKeysCount = await cacheService.RemoveAsync(cacheKeys, cancellationTokenSource.Token);

            Assert.Equal(cacheKeys.Count(), removedKeysCount);
        }

        [Theory]
        [MemberData(
            nameof(SearchCacheServiceTestScenarios.Search_OriginDateTime_ScenarioDatas),
            parameters: [10, 1000],
            MemberType = typeof(SearchCacheServiceTestScenarios)
        )]
        public async Task SearchAsync_OriginDateTime_Test(Route[] routes, SearchRequest request, int expectedResultCount)
        {
            var factory = new LoggerFactory();

            var logger = factory.CreateLogger<Services.v1.SearchCacheService>();

            var cacheService = new Services.v1.SearchCacheService(logger);

            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(2000);

            await cacheService.AddAsync(routes, cancellationTokenSource.Token);

            var searchResponse = await cacheService.SearchAsync(request, cancellationTokenSource.Token);

            Assert.Equal(expectedResultCount, searchResponse.Routes.Length);
        }

        [Theory]
        [MemberData(
            nameof(SearchCacheServiceTestScenarios.Search_DestinationDateTime_ScenarioDatas),
            parameters: [10, 1000],
            MemberType = typeof(SearchCacheServiceTestScenarios)
        )]
        public async Task SearchAsync_DestinationDateTime_Test(Route[] routes, SearchRequest request, int expectedResultCount)
        {
            var factory = new LoggerFactory();

            var logger = factory.CreateLogger<Services.v1.SearchCacheService>();

            var cacheService = new Services.v1.SearchCacheService(logger);

            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(2000);

            await cacheService.AddAsync(routes, cancellationTokenSource.Token);

            var searchResponse = await cacheService.SearchAsync(request, cancellationTokenSource.Token);

            Assert.Equal(expectedResultCount, searchResponse.Routes.Length);
        }

        [Theory]
        [MemberData(
            nameof(SearchCacheServiceTestScenarios.Search_Price_ScenarioDatas),
            parameters: [10, 1000],
            MemberType = typeof(SearchCacheServiceTestScenarios)
        )]
        public async Task SearchAsync_Price_Test(Route[] routes, SearchRequest request, int expectedResultCount)
        {
            var factory = new LoggerFactory();

            var logger = factory.CreateLogger<Services.v1.SearchCacheService>();

            var cacheService = new Services.v1.SearchCacheService(logger);

            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(2000);

            await cacheService.AddAsync(routes, cancellationTokenSource.Token);

            var searchResponse = await cacheService.SearchAsync(request, cancellationTokenSource.Token);

            Assert.Equal(expectedResultCount, searchResponse.Routes.Length);
        }

        [Theory]
        [MemberData(
            nameof(SearchCacheServiceTestScenarios.Search_MinTimeLimit_ScenarioDatas),
            parameters: [10, 1000],
            MemberType = typeof(SearchCacheServiceTestScenarios)
        )]
        public async Task SearchAsync_MinTimeLimit_Test(Route[] routes, SearchRequest request, int expectedResultCount)
        {
            var factory = new LoggerFactory();

            var logger = factory.CreateLogger<Services.v1.SearchCacheService>();

            var cacheService = new Services.v1.SearchCacheService(logger);

            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(2000);

            await cacheService.AddAsync(routes, cancellationTokenSource.Token);

            var searchResponse = await cacheService.SearchAsync(request, cancellationTokenSource.Token);

            Assert.Equal(expectedResultCount, searchResponse.Routes.Length);
        }
    }
}