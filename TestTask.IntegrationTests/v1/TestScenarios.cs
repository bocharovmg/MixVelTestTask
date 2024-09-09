using TestTask.Domain.Contracts.v1.Dtos;
using TestTask.Domain.Contracts.v1.Requests;


namespace TestTask.IntegrationTests.v1
{
    public static class TestScenarios
    {
        public static IEnumerable<object[]> Search_ScenarioDatas()
        {
            return [
                [
                    new SearchRequest
                    {
                        Origin = "TestOrigin",
                        OriginDateTime = DateTime.Now,
                        Destination = "TestDestination"
                    },

                    1 // expectedRoutesCount
                ],
                [
                    new SearchRequest
                    {
                        Origin = "TestOrigin",
                        OriginDateTime = DateTime.Now,
                        Destination = "TestDestination",
                        Filters = new SearchFilters
                        {
                            DestinationDateTime = DateTime.Now,
                        }
                    },

                    1 // expectedRoutesCount
                ],
                [
                    new SearchRequest
                    {
                        Origin = "TestOrigin",
                        OriginDateTime = DateTime.Now,
                        Destination = "TestDestination",
                        Filters = new SearchFilters
                        {
                            MaxPrice = 10000
                        }
                    },

                    1 // expectedRoutesCount
                ],
                [
                    new SearchRequest
                    {
                        Origin = "TestOrigin",
                        OriginDateTime = DateTime.Now,
                        Destination = "TestDestination",
                        Filters = new SearchFilters
                        {
                            MinTimeLimit = DateTime.MinValue.AddSeconds(60),
                        }
                    },

                    1 // expectedRoutesCount
                ],
                [
                    new SearchRequest
                    {
                        Origin = "TestOrigin",
                        OriginDateTime = DateTime.Now,
                        Destination = "TestDestination",
                        Filters = new SearchFilters
                        {
                            OnlyCached = true,
                        }
                    },

                    1 // expectedRoutesCount
                ]
            ];
        }
    }
}
