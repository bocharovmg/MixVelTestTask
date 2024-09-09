using TestTask.Providers.ProviderOne.v1.Dtos;


namespace TestTask.Providers.ProviderOne.v1.Responses
{
    public class ProviderOneSearchResponse
    {
        // Mandatory
        // Array of routes
        public ProviderOneRoute[] Routes { get; set; } = Array.Empty<ProviderOneRoute>();
    }
}
