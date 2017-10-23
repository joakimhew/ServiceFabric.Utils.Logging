using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServiceFabric.Utils.Logging.LoggerProviders;
using ServiceFabric.Utils.Logging.Middlewares;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceFabric.Utils.Logging.Extensions
{
    public static class SentryExtensions
    {
        public static IApplicationBuilder UseSentryGlobally(this IApplicationBuilder app, string dsn)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            var options = new SentryOptions
            {
                Dsn = dsn,
                EventId = 0,
                LogLevel = LogLevel.Critical
            };

            return app.UseMiddleware<SentryMiddleware>(options);
        }

        public static ILoggerFactory AddCriticalSentryLogger(this ILoggerFactory loggerFactory, IHttpContextAccessor contextAccessor, string dsn)
        {
            var options = new SentryOptions
            {
                Dsn = dsn,
                EventId = 0,
                LogLevel = LogLevel.Critical
            };

            loggerFactory.AddProvider(new SentryLoggerProvider(options, contextAccessor));

            return loggerFactory;
        } 
    }
}
