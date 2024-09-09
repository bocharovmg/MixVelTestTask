using AutoMapper;
using TestTask.Domain.Services.v1;
using TestTask.Application.Services.v1;
using TestTask.Providers.ProviderOne.v1;
using TestTask.Providers.ProviderTwo.v1;


namespace TestTask.Api.Configuration
{
    public static class ServicesRegistration
    {
        /// <summary>
        /// Services registration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationRoot"></param>
        public static void Register(IServiceCollection services, IConfiguration configurationRoot)
        {
            RegisterSingletonServices(services);

            RegisterRuntimeServices(services, configurationRoot);
        }

        /// <summary>
        /// Register singleton services
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterSingletonServices(IServiceCollection services)
        {
            services.AddSingleton<ISearchCacheService, SearchCacheService>();
        }

        /// <summary>
        /// Register runtime services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationRoot"></param>
        private static void RegisterRuntimeServices(IServiceCollection services, IConfiguration configurationRoot)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<Providers.ProviderOne.v1.Profiles.SearchRequestProfile>();
                cfg.AddProfile<Providers.ProviderOne.v1.Profiles.SearchResponseProfile>();
                cfg.AddProfile<Providers.ProviderTwo.v1.Profiles.SearchRequestProfile>();
                cfg.AddProfile<Providers.ProviderTwo.v1.Profiles.SearchResponseProfile>();
            });

            services.AddHttpClient();

            object thirdPartyServiceProvidersKey = "third-party-services";

            RegisterSearchProviders(services, configurationRoot, thirdPartyServiceProvidersKey);

            services.AddScoped<ISearchService>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<SearchService>>();

                var searchCacheService = serviceProvider.GetRequiredService<ISearchCacheService>();

                var searchProviderServices = serviceProvider.GetRequiredKeyedService<IEnumerable<ISearchService>>(thirdPartyServiceProvidersKey);

                return new SearchService(logger, searchCacheService, searchProviderServices);
            });
        }

        /// <summary>
        /// Register search providers
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationRoot"></param>
        private static void RegisterSearchProviders(
            IServiceCollection services,
            IConfiguration configurationRoot,
            object serviceKey
        )
        {
            var searchProvidersConfiguration = configurationRoot.GetRequiredSection("SearchProviders");

            RegisterHttpSearchProvider(services, searchProvidersConfiguration, serviceKey, "ProviderOne", BuildSearchProviderService<ProviderOneSearchService>);

            RegisterHttpSearchProvider(services, searchProvidersConfiguration, serviceKey, "ProviderTwo", BuildSearchProviderService<ProviderTwoSearchService>);
        }

        private delegate ISearchService BuildSearchProviderServiceDelegate(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            string providerName
        );

        /// <summary>
        /// Register http search provider
        /// </summary>
        /// <param name="services"></param>
        /// <param name="searchProvidersConfiguration"></param>
        /// <param name="providerName"></param>
        /// <param name="buildProviderSearchServiceDelegate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private static void RegisterHttpSearchProvider(
            IServiceCollection services,
            IConfiguration searchProvidersConfiguration,
            object serviceKey,
            string providerName,
            BuildSearchProviderServiceDelegate buildProviderSearchServiceDelegate
        )
        {
            var configuration = searchProvidersConfiguration.GetRequiredSection(providerName);

            var address = configuration["Address"];

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException($"Configuration value {nameof(address)} is not specified");
            }

            services
                .AddHttpClient(
                    providerName,
                    client =>
                    {
                        client.BaseAddress = new Uri(address);
                    }
                );

            services
                .AddKeyedScoped(
                    serviceKey,
                    (serviceProvider, _) =>
                        buildProviderSearchServiceDelegate(
                            serviceProvider,
                            configuration,
                            providerName
                        )
                );
        }

        /// <summary>
        /// Creates a new <see cref="ISearchService"/> instance using the full name of the given type <typeparamref name="TSearchProviderService"/>.
        /// </summary>
        /// <typeparam name="ISearchService"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="configuration"></param>
        /// <param name="providerName"></param>
        /// <returns>Returns a <typeparamref name="TSearchProviderService"/> instance.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static ISearchService BuildSearchProviderService<TSearchProviderService>(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            string providerName
        ) where TSearchProviderService : ISearchService
        {
            var logger = serviceProvider.GetRequiredService<ILogger<TSearchProviderService>>();

            var mapper = serviceProvider.GetRequiredService<IMapper>();

            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            var result = (ISearchService?)Activator
                .CreateInstance(
                    typeof(TSearchProviderService),
                    logger,
                    mapper,
                    configuration,
                    httpClientFactory,
                    providerName
                );

            if (result == null)
            {
                throw new ArgumentNullException($"Failed to construct provider {providerName}");
            }

            return result;
        }
    }
}
