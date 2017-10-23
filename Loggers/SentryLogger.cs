using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharpRaven;
using SharpRaven.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabric.Utils.Logging.Loggers
{
    public class SentryLogger : ILogger
    {
        private readonly SentryOptions _options;
        private readonly HttpContext _context;

        public SentryLogger(SentryOptions options, HttpContext context)
        {
            _options = options;
            _context = context;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _options.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(!IsEnabled(logLevel))
            {
                return;
            }

            if(_options.EventId == 0 || _options.EventId == eventId.Id)
            {
                try
                {
                    var ravenClient = new RavenClient(
                      dsn                   : _options.Dsn,
                      sentryRequestFactory  : new AspNetCoreSentryRequestFactory(_context));

                    ravenClient.Capture(new SentryEvent(exception));
                }

                catch (Exception ex)
                {
                    throw new Exception($"Logging to sentry failed {ex}", exception);
                }
            }
        }
    }
}
