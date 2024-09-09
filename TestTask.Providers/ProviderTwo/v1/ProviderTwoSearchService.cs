using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestTask.Providers.Common.v1;
using TestTask.Providers.ProviderTwo.v1.Requests;
using TestTask.Providers.ProviderTwo.v1.Responses;


namespace TestTask.Providers.ProviderTwo.v1
{
    /// <summary>
    /// Represents a http search provider service <see cref="ProviderTwoSearchService"/> with a request of type <see cref="ProviderTwoSearchRequest"/> and a response of type <see cref="ProviderTwoSearchResponse"/>
    /// </summary>
    public class ProviderTwoSearchService : TemplateHttpSearchProviderService<ProviderTwoSearchRequest, ProviderTwoSearchResponse>
    {
        public ProviderTwoSearchService(
            ILogger<ProviderTwoSearchService> logger,
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
