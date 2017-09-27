using SharpRaven;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceFabric.Utils.Logging
{
    public class SentryLoggerConfiguration
    {
        public IRavenClient RavenClient { get; set; }
    }
}
