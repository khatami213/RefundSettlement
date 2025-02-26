using System;
using log4net;

namespace Logging.Log4net
{
    public class Log4NetLogger : BaseLogger
    {
        protected ILog Logger;
        public override void Debug(string log)
        {
            Logger.Debug(log);            
        }

        public override void Error(string log)
        {
            Logger.Error(log);
        }

        public override void Fatal(string log)
        {
            Logger.Fatal(log);
        }

        public override void Info(string log)
        {
            Logger.Info(log);
        }

        public override void Warn(string log)
        {
            Logger.Warn(log);
        }

        public Log4NetLogger(string loggerName):base(loggerName)
        {
            Logger = log4net.LogManager.GetLogger(loggerName);
        }

        public Log4NetLogger(Type className) : base(className)
        {
            Logger = log4net.LogManager.GetLogger(className);
        }
    }
}
