using Microsoft.Extensions.Logging;
using SharpRaven.Data;
using System;

namespace ServiceFabric.Utils.Logging.Loggers
{

    public class SentryLogger : ILogger
    {
        private readonly SentryLoggerConfiguration _configuration;

        public SentryLogger(SentryLoggerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }


        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Critical;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            _configuration.RavenClient.Capture(new SentryEvent(exception));
        }
    }
}
