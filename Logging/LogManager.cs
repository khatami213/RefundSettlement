using System;
using Logging.Log4net;

namespace Logging
{
    public class LogManager
    {
        public static ILogger GetLogger(string loggerName)
        {
            return new Log4NetLogger(loggerName);
        }

        public static ILogger GetLogger(Type className)
        {
            return new Log4NetLogger(className);
        }
    }
}
