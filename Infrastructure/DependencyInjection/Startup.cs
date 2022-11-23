using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DependencyInjection
{
    public class Startup
    {
        private static readonly ServiceProvider _serviceProvider = new ContainerBuilder().Build();

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<Startup> _logger;

        public Startup()
        {
            _loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
            _logger = _loggerFactory.CreateLogger<Startup>();
        }
    }
}