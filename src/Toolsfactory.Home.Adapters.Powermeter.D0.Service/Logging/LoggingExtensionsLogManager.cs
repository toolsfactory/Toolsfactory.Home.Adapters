using Microsoft.Extensions.Logging;
using System;
using Tiveria.Common.Logging;

namespace Toolsfactory.Home.Adapters.Powermeter.D0
{
    public class LoggingExtensionsLogManager : ILogManager
    {
        private readonly ILoggerFactory _logFactory;

        public LoggingExtensionsLogManager(Microsoft.Extensions.Logging.ILoggerFactory logFactory)
        {
            _logFactory = logFactory;
        }
        public Common.Logging.ILogger GetLogger(string name)
        {
            return new LoggingExtensionsLogger(_logFactory.CreateLogger(name));
        }

        public Common.Logging.ILogger GetLogger(Type type)
        {
            return new LoggingExtensionsLogger(_logFactory.CreateLogger(nameof(type)));
        }
    }
}
