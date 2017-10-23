using Microsoft.Extensions.Logging;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceFabric.Utils.Logging
{
    public class SentryOptions
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Critical;
        public int EventId { get; set; } = 0;
        public string Dsn { get; set; }
    }
}
