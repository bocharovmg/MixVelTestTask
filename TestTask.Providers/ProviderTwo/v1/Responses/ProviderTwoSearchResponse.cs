using TestTask.Providers.ProviderTwo.v1.Dtos;


namespace TestTask.Providers.ProviderTwo.v1.Responses
{
    public class ProviderTwoSearchResponse
    {
        // Mandatory
        // Array of routes
        public ProviderTwoRoute[] Routes { get; set; } = Array.Empty<ProviderTwoRoute>();
    }
}
