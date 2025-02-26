using System;

namespace Logging
{
    public abstract class BaseLogger : ILogger
    {
        protected string LoggerName;

        protected Type ClassName;

        public abstract void Debug(string log);

        public abstract void Error(string log);

        public abstract void Fatal(string log);

        public abstract void Info(string log);

        public abstract void Warn(string log);

        protected BaseLogger(string loggerName)
        {
            LoggerName = loggerName;
        }

        protected BaseLogger(Type className)
        {
            ClassName = className;
        }
    }
}
