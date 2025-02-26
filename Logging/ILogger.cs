namespace Logging
{
    public interface ILogger
    {
        void Info(string log);
        void Debug(string log);
        void Warn(string log);
        void Error(string log);
        void Fatal(string log);
    }
}
