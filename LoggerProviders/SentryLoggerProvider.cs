using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceFabric.Utils.Logging.Loggers;
using System.Collections.Concurrent;

namespace ServiceFabric.Utils.Logging.LoggerProviders
{
    public class SentryLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, SentryLogger> _loggers =
            new ConcurrentDictionary<string, SentryLogger>();

        private readonly SentryOptions _options;
        private readonly IHttpContextAccessor _contextAccessor;

        public SentryLoggerProvider(SentryOptions options, IHttpContextAccessor contextAccessor)
        {
            _options = options;
            _contextAccessor = contextAccessor;
        }


        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, new SentryLogger(_options, _contextAccessor.HttpContext));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
