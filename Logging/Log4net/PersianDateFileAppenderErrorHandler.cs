using System;
using log4net.Core;

namespace Logging.Log4net
{
    public class PersianDateFileAppenderErrorHandler : IErrorHandler
    {
        private bool _firstErrorPrinted = false;

        private void WriteFirstErrorToConsole(string message)
        {
            if (!_firstErrorPrinted)
            {
                Console.WriteLine(message);
                _firstErrorPrinted = true;
            }
        }

        public void Error(string message)
        {
            WriteFirstErrorToConsole(message);
        }

        public void Error(string message, Exception e)
        {
            WriteFirstErrorToConsole(message + " : " + e.Message);
        }

        public void Error(string message, Exception e, ErrorCode errorCode)
        {
            WriteFirstErrorToConsole(message + ", Error code : " + errorCode + ", " + e.Message);
        }
    }
}
