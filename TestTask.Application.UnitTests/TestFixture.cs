namespace TestTask.Application.UnitTests
{
    public class TestFixture : IDisposable
    {
        private IServiceProvider? _serviceProvider = null;

        public IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = ServiceProviderFactory.CreateServiceProvider();
                }

                return _serviceProvider;
            }
        }

        public TestFixture()
        {
        }

        public void Dispose()
        {
        }
    }
}
