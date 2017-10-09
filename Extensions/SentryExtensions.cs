using Microsoft.AspNetCore.Builder;
using ServiceFabric.Utils.Logging.Middlewares;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceFabric.Utils.Logging.Extensions
{
    public static class SentryExtensions
    {
        public static IApplicationBuilder UseSentry(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<SentryMiddleware>();
        }
    }
}
