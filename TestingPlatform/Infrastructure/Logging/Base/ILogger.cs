namespace TestingPlatform.Infrastructure.Logging.Base
{
    internal interface ILogger
    {
        public static void Debug(string message) { }
        public static void Info(string message) { }
        public static void Warning(string message) { }
        public static void Error(string message) { }
        public static void Critical(string message) { }
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
