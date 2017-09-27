#if NET461

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabric.Utils.Logging
{
    public interface ILogger
    {
        bool IsEnabled(LogLevel logLevel);

        void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);
    }
}

#endif