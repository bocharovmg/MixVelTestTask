using TestTask.Domain.Contracts.v1.Dtos;
using TestTask.Domain.Contracts.v1.Requests;


namespace TestTask.Application.UnitTests.SearchCacheService
{
    public static class SearchCacheServiceTestScenarios
    {
        /// <summary>
        /// Create a new <see cref="Route"/> with given parameters:
        /// </summary>
        /// <param name="namePrefix">Prefix for the Origin and Destination.</param>
        /// <param name="price">The price of the <see cref="Route"/></param>
        /// <param name="originDateTime"></param>
        /// <param name="destinationDateTime"></param>
        /// <param name="timeLimit">Time limit in seconds</param>
        /// <returns></returns>
        public static Route BuildRoute(
            string namePrefix,
            decimal price,
            DateTime originDateTime,
            DateTime destinationDateTime,
            int timeLimit
        )
        {
            var minTime = DateTime.MinValue;

            var result = new Route
            {
                Origin = $"{namePrefix}_Origin",
                Destination = $"{namePrefix}_Destination",
                OriginDateTime = originDateTime,
                DestinationDateTime = destinationDateTime,
                Price = price,
                TimeLimit = minTime.AddSeconds(timeLimit)
            };

            return result;
        }

        public static IEnumerable<object[]> NRoutesInOneTime_ScenarioDatas(int repeats, int routeCount)
        {
            var result = new List<object[]>(repeats * routeCount);

            for (var repeatIndex = 0; repeatIndex < repeats; repeatIndex++)
            {
                var routes = PrepareRoutes(routeCount);

                result.Add([
                    routes
                ]);
            }

            return result;
        }

        public static IEnumerable<object[]> Search_OriginDateTime_ScenarioDatas(int repeats, int routeCount)
        {
            var result = new List<object[]>(repeats * routeCount);

            for (var repeatIndex = 0; repeatIndex < repeats; repeatIndex++)
            {
                var routes = PrepareRoutes(routeCount);

                var randomizer = new Random(repeatIndex);

                var routeIndex = randomizer.Next(routes.Length - 1);

                var route = routes[routeIndex];

                var searchRequest = new SearchRequest
                {
                    Origin = route.Origin,
                    Destination = route.Destination,
                    OriginDateTime = route.OriginDateTime
                };

                var expectedResultCount = routes
                    .Where(route =>
                        route.Origin == searchRequest.Origin
                        && route.Destination == searchRequest.Destination
                        && route.OriginDateTime == searchRequest.OriginDateTime
                    )
                    .Count();

                result.Add([
                    routes,
                    searchRequest,
                    expectedResultCount
                ]);
            }

            return result;
        }

        public static IEnumerable<object[]> Search_DestinationDateTime_ScenarioDatas(int repeats, int routeCount)
        {
            var result = new List<object[]>(repeats * routeCount);

            for (var repeatIndex = 0; repeatIndex < repeats; repeatIndex++)
            {
                var routes = PrepareRoutes(routeCount);

                var randomizer = new Random(repeatIndex);

                var routeIndex = randomizer.Next(routes.Length - 1);

                var route = routes[routeIndex];

                var searchRequest = new SearchRequest
                {
                    Origin = route.Origin,
                    Destination = route.Destination,
                    OriginDateTime = route.OriginDateTime,
                    Filters = new SearchFilters
                    {
                        DestinationDateTime = route.DestinationDateTime,
                    }
                };

                var expectedResultCount = routes
                    .Where(route =>
                        route.Origin == searchRequest.Origin
                        && route.Destination == searchRequest.Destination
                        && route.OriginDateTime == searchRequest.OriginDateTime
                        && route.DestinationDateTime == searchRequest.Filters.DestinationDateTime
                    )
                    .Count();

                result.Add([
                    routes,
                    searchRequest,
                    expectedResultCount
                ]);
            }

            return result;
        }

        public static IEnumerable<object[]> Search_Price_ScenarioDatas(int repeats, int routeCount)
        {
            var result = new List<object[]>(repeats * routeCount);

            for (var repeatIndex = 0; repeatIndex < repeats; repeatIndex++)
            {
                var routes = PrepareRoutes(routeCount);

                var randomizer = new Random(repeatIndex);

                var routeIndex = randomizer.Next(routes.Length - 1);

                var route = routes[routeIndex];

                var searchRequest = new SearchRequest
                {
                    Origin = route.Origin,
                    Destination = route.Destination,
                    OriginDateTime = route.OriginDateTime,
                    Filters = new SearchFilters
                    {
                        MaxPrice = route.Price,
                    }
                };

                var expectedResultCount = routes
                    .Where(route =>
                        route.Origin == searchRequest.Origin
                        && route.Destination == searchRequest.Destination
                        && route.OriginDateTime == searchRequest.OriginDateTime
                        && route.Price <= searchRequest.Filters.MaxPrice
                    )
                    .Count();

                result.Add([
                    routes,
                    searchRequest,
                    expectedResultCount
                ]);
            }

            return result;
        }

        public static IEnumerable<object[]> Search_MinTimeLimit_ScenarioDatas(int repeats, int routeCount)
        {
            var result = new List<object[]>(repeats * routeCount);

            for (var repeatIndex = 0; repeatIndex < repeats; repeatIndex++)
            {
                var routes = PrepareRoutes(routeCount);

                var randomizer = new Random(repeatIndex);

                var routeIndex = randomizer.Next(routes.Length - 1);

                var route = routes[routeIndex];

                var searchRequest = new SearchRequest
                {
                    Origin = route.Origin,
                    Destination = route.Destination,
                    OriginDateTime = route.OriginDateTime,
                    Filters = new SearchFilters
                    {
                        MinTimeLimit = route.TimeLimit
                    }
                };

                var expectedResultCount = routes
                    .Where(route =>
                        route.Origin == searchRequest.Origin
                        && route.Destination == searchRequest.Destination
                        && route.OriginDateTime == searchRequest.OriginDateTime
                        && route.TimeLimit >= searchRequest.Filters.MinTimeLimit
                    )
                    .Count();

                result.Add([
                    routes,
                    searchRequest,
                    expectedResultCount
                ]);
            }

            return result;
        }

        private static Route[] PrepareRoutes(
            int routeCount,
            decimal minPrice = 1,
            decimal maxPrice = 1000000,
            int minTimeLimit = 60,
            int maxTimeLimit = 120
        )
        {
            var result = new Route[routeCount];

            for (var routeIndex = 0; routeIndex < routeCount; routeIndex++)
            {
                var randomizer = new Random(routeIndex);

                var routePrefix = routeIndex / 10;

                var originDateTime = DateTime.Now.AddDays(routeIndex % 10);

                var destinationDateTime = DateTime.Now.AddDays(routeIndex % 10 * 2);

                decimal price = randomizer.Next((int)(minPrice * 100), (int)(maxPrice * 100)) / 100m;

                int timeLimit = randomizer.Next(minTimeLimit, maxTimeLimit);

                result[routeIndex] = BuildRoute(routePrefix.ToString(), price, originDateTime, destinationDateTime, timeLimit);
            }

            return result;
        }
    }
}
