using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using TestTask.Domain.Contracts.v1.Requests;
using TestTask.Domain.Contracts.v1.Responses;


namespace TestTask.IntegrationTests.v1
{
    public class Tests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public Tests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [MemberData(
            nameof(TestScenarios.Search_ScenarioDatas),
            MemberType = typeof(TestScenarios)
        )]
        public async Task Search_Test(SearchRequest request, int expectedRoutesCount)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(5000);

            var client = _factory.CreateClient();

            var requestUri = "/routes/v1/search";

            var searchResponseMessage = await client.PostAsJsonAsync(requestUri, request, cancellationTokenSource.Token);

            var searchResponse = await searchResponseMessage.Content.ReadFromJsonAsync<SearchResponse>(cancellationToken: cancellationTokenSource.Token);

            Assert.NotNull(searchResponse);

            Assert.Equal(expectedRoutesCount, searchResponse.Routes.Length);
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task Ping_Test(HttpStatusCode expectedStatusCode)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(5000);

            var client = _factory.CreateClient();

            var requestUri = "/routes/v1/ping";

            var searchResponseMessage = await client.GetAsync(requestUri, cancellationTokenSource.Token);

            Assert.Equal(expectedStatusCode, searchResponseMessage.StatusCode);
        }
    }
}