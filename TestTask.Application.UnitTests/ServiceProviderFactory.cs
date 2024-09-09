using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestTask.Api.Configuration;


namespace TestTask.Application.UnitTests
{
    internal static class ServiceProviderFactory
    {
        public static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            var configuration = BuildConfiguration();

            ServicesRegistration.Register(services, configuration);

            return services.BuildServiceProvider();
        }

        private static IConfiguration BuildConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();

            var path = Directory.GetCurrentDirectory();

            configurationBuilder.AddJsonFile($"{path}/appsettings.json");

            var configuration = configurationBuilder.Build();

            return configuration;
        }
    }
}
