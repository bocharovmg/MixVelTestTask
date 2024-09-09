using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TestTask.Domain.Contracts.v1.Requests;
using TestTask.Domain.Contracts.v1.Responses;
using TestTask.Domain.Services.v1;


namespace TestTask.Providers.Common.v1
{
    /// <summary>
    /// Represents a template for the http search provider service
    /// <see cref="TemplateHttpSearchProviderService{TSearchProviderRequest, TSearchProviderResponse}"/>
    /// </summary>
    /// <typeparam name="TSearchProviderRequest">Type of request</typeparam>
    /// <typeparam name="TSearchProviderResponse">Type of response</typeparam>
    public abstract class TemplateHttpSearchProviderService<TSearchProviderRequest, TSearchProviderResponse> : ISearchService
    {
        private readonly ILogger _logger;

        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        private readonly string _httpProviderName;

        private readonly IHttpClientFactory _httpClientFactory;

        public TemplateHttpSearchProviderService(
            ILogger logger,
            IMapper mapper,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            string httpProviderName
        )
        {
            _logger = logger;

            _mapper = mapper;

            _configuration = configuration;

            _httpClientFactory = httpClientFactory;

            _httpProviderName = httpProviderName;
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            var searchUri = _configuration[$"searchUri"] ?? "api/v1/search";

            using var httpClient = _httpClientFactory.CreateClient(_httpProviderName);

            try
            {
                var providerSearchRequest = _mapper.Map<TSearchProviderRequest>(request);

                var providerSearchResponse = await httpClient.PostAsJsonAsync(searchUri, providerSearchRequest, cancellationToken);

                providerSearchResponse.EnsureSuccessStatusCode();

                var providerSearchResult = providerSearchResponse.Content.ReadFromJsonAsync<TSearchProviderResponse>(cancellationToken);

                var result = _mapper.Map<SearchResponse>(providerSearchResult);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error search routes: {Ex}", ex);
            }

            return new SearchResponse();
        }

        public virtual async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            var pingUri = _configuration[$"pingUri"] ?? "api/v1/ping";

            using var client = _httpClientFactory.CreateClient(_httpProviderName);

            try
            {
                var providerPingResponse = await client.GetAsync(pingUri, cancellationToken);

                return providerPingResponse.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error check the service {BaseAddress} availability: {Ex}", client.BaseAddress, ex);
            }

            return false;
        }
    }
}
