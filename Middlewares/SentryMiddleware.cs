using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SharpRaven;
using SharpRaven.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabric.Utils.Logging.Middlewares
{
    public class SentryMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SentryOptions _options;

        public SentryMiddleware(
            RequestDelegate next,
            SentryOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(_options.Dsn))
                {
                    try
                    {
                        var sentry = new RavenClient(
                            _options.Dsn,
                            sentryRequestFactory: new AspNetCoreSentryRequestFactory(context));
                        await sentry.CaptureAsync(new SentryEvent(ex));
                    }
                    catch (Exception sentryEx)
                    {
                        throw new Exception($"Logging to sentry failed, {sentryEx}", ex);
                    }
                }

                throw;
            }
        }
    }
}
