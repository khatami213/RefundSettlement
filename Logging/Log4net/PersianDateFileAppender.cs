using System;
using System.Globalization;
using log4net.Appender;
using log4net.Core;

namespace Logging.Log4net
{
    public class PersianDateFileAppender: FileAppender
    {
        private readonly PersianCalendar _persianCalendar = new PersianCalendar();
        private DateTime _fileDate = DateTime.MinValue;
        private string _filename;

        public string Path { get; set; }
        public string Prefix { get; set; } = "";
        public string FullFileName { get; private set; }


        public PersianDateFileAppender()
        {
            ErrorHandler = new PersianDateFileAppenderErrorHandler();
            ImmediateFlush = true;            
        }

        public sealed override IErrorHandler ErrorHandler
        {
            get => base.ErrorHandler;
            set => base.ErrorHandler = value;
        }

        private void ChangeFile()
        {
            _fileDate = DateTime.Now.Date;
            _filename = $"{Prefix}-{_persianCalendar.GetYear(_fileDate):0000}-{_persianCalendar.GetMonth(_fileDate):00}-{_persianCalendar.GetDayOfMonth(_fileDate):00}.log";
            FullFileName = _filename;

            if (!string.IsNullOrWhiteSpace(Path))
            {
                FullFileName = Path + "\\" + _filename;
            }

            CloseFile();

            OpenFile(FullFileName, AppendToFile);            
        }
        protected override void Append(LoggingEvent[] loggingEvents)
        {
            if(DateTime.Now.Date > _fileDate)
            {
                ChangeFile();
            }
            base.Append(loggingEvents);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (DateTime.Now.Date > _fileDate)
            {
                ChangeFile();
            }
            base.Append(loggingEvent);
        }
    }
}
