using Microsoft.Extensions.Logging;
using ServiceFabric.Utils.Logging.Loggers;
using System.Collections.Concurrent;

namespace ServiceFabric.Utils.Logging.LoggerProviders
{
    public class SentryLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, SentryLogger> _loggers =
            new ConcurrentDictionary<string, SentryLogger>();

        private readonly SentryLoggerConfiguration _configuration;

        public SentryLoggerProvider(SentryLoggerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, new SentryLogger(_configuration));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}