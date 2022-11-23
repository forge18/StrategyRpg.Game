using Serilog;
using Serilog.Enrichers;
using Serilog.Exceptions;

namespace Infrastructure.Logging
{
    public class LogSettings
    {
        public void ConfigureLogger()
        {
            var isLoggingEnabled = true;
            Log.Logger = new LoggerConfiguration()
                .Filter.ByExcluding(_ => !isLoggingEnabled)
                .Enrich.FromLogContext()
                .Enrich.With(new ThreadIdEnricher())
                .Enrich.WithExceptionDetails()
                .Enrich.WithDemystifiedStackTraces()
                .MinimumLevel.Debug()
                .WriteTo.File(
                "Infrastructure/Logging/Logs/Log.txt",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] ({SourceContext}) {Message}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7
                )
                .CreateLogger();
        }
    }
}