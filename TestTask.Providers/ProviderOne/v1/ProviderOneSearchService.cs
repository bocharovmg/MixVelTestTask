using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestTask.Providers.Common.v1;
using TestTask.Providers.ProviderOne.v1.Responses;
using TestTask.Providers.ProviderOne.v1.Requests;


namespace TestTask.Providers.ProviderOne.v1
{
    /// <summary>
    /// Represents a http search provider service <see cref="ProviderOneSearchService"/> with a request of type <see cref="ProviderOneSearchRequest"/> and a response of type <see cref="ProviderOneSearchResponse"/>
    /// </summary>
    public class ProviderOneSearchService : TemplateHttpSearchProviderService<ProviderOneSearchRequest, ProviderOneSearchResponse>
    {
        public ProviderOneSearchService(
            ILogger<ProviderOneSearchService> logger,
            IMapper mapper,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            string httpProviderName
        ) :
            base(logger, mapper, configuration, httpClientFactory, httpProviderName)
        {
        }
    }
}
