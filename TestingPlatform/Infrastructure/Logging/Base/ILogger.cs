namespace TestingPlatform.Infrastructure.Logging.Base
{
    internal interface ILogger
    {
        public LogLevel Level { get; set; }
        public void Debug(string message);
        public void Info(string message);
        public void Warning(string message);
        public void Error(string message);
        public void Critical(string message);
    }
    enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Critical,
    }
}
