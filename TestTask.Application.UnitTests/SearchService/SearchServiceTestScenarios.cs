using TestTask.Application.UnitTests.SearchCacheService;


namespace TestTask.Application.UnitTests.SearchService
{
    public static class SearchServiceTestScenarios
    {
        public static IEnumerable<object[]> Search_OriginDateTime_RandomScenarioDatas(int routeCount)
        {
            return SearchCacheServiceTestScenarios.Search_OriginDateTime_ScenarioDatas(1, routeCount);
        }

        public static IEnumerable<object[]> Search_DestinationDateTime_RandomScenarioDatas(int routeCount)
        {
            return SearchCacheServiceTestScenarios.Search_DestinationDateTime_ScenarioDatas(1, routeCount);
        }

        public static IEnumerable<object[]> Search_Price_RandomScenarioDatas(int routeCount)
        {
            return SearchCacheServiceTestScenarios.Search_Price_ScenarioDatas(1, routeCount);
        }

        public static IEnumerable<object[]> Search_MinTimeLimit_RandomScenarioDatas(int routeCount)
        {
            return SearchCacheServiceTestScenarios.Search_MinTimeLimit_ScenarioDatas(1, routeCount);
        }
    }
}
