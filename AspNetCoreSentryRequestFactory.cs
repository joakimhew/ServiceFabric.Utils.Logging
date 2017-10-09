using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SharpRaven.Data;
using SharpRaven.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServiceFabric.Utils.Logging
{
    // Grabbed from gist at: https://gist.github.com/jabrown85/82baf795fc3853266118996fbd30dd50
    public class AspNetCoreSentryRequestFactory : ISentryRequestFactory
    {
        public AspNetCoreSentryRequestFactory(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        private HttpContext HttpContext { get; }

        public ISentryRequest Create()
        {
            var request = new SentryRequest
            {
                Url = HttpContext.Request.Path.ToString(),
                Method = HttpContext.Request.Method,
                Environment = new Dictionary<string, string>(), // NOTE: we did not want or need this
                Headers = Convert(HttpContext.Request.Headers),
                Cookies = HttpContext.Request.Cookies.ToDictionary(x => x.Key, x => x.Value),
                Data = BodyConvert(),
                QueryString = HttpContext.Request.QueryString.ToString(),
            };

            return request;
        }

        private static IDictionary<string, string> Convert(IEnumerable<KeyValuePair<string, StringValues>> collection)
        {
            return collection.ToDictionary(x => x.Key, x => string.Join(" ", x.Value.ToArray()));
        }

        private object BodyConvert()
        {
            try
            {
                dynamic wrapper = new ExpandoObject();
                wrapper.Request = new DynamicHttpContextRequest(HttpContext);
                return HttpRequestBodyConverter.Convert(wrapper);
            }
            catch (Exception exception)
            {
                SystemUtil.WriteError(exception);
            }

            return null;
        }

        /// <summary>
        /// This code is mocking the HttpContext so that it can look like the original System.Web HttpContext that Sentry is still expecting, even though they
        /// code against a dynamic, which is why this whole thing can work in the first place.
        ///
        /// We've only implemented the properties that are expected today.
        /// </summary>
        public class DynamicHttpContextRequest
        {
            private readonly HttpContext _context;
            private readonly MemoryStream _stream;

            public DynamicHttpContextRequest(HttpContext context)
            {
                _context = context;

                if (_context.Request.Body == null) return;

                using (var bodyReader = new StreamReader(_context.Request.Body))
                {
                    var body = bodyReader.ReadToEndAsync().Result;

                    _stream = new MemoryStream(Encoding.UTF8.GetBytes(body));
                }
            }

            public string ContentType => _context.Request.ContentType;

            public NameValueCollection Form
            {
                get
                {
                    var coll = new NameValueCollection();
                    foreach (var x in _context.Request.Form)
                    {
                        coll.Add(x.Key, x.Value);
                    }

                    return coll;
                }
            }

            public Stream InputStream => _stream;
        }
    }
}
