using System;
using TestingPlatform.Infrastructure.Logging.Base;
using TestingPlatform.Infrastructure.Logging.Handlers;

namespace TestingPlatform.Infrastructure.Logging
{
    public class Logger : ILogger
    {
        private static HandlerManager handlerManager { get; set; }
        private static Logger _instance = null;
        private static readonly object padlock = new(); // thread-safety

        private Logger()
        {
        }

        public static Logger Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Logger();
                        handlerManager = new HandlerManager();
                        handlerManager.AddHandler(new ConsoleHandler());
                        handlerManager.AddHandler(new FileHandler(AppDomain.CurrentDomain.BaseDirectory));
                    }
                    return _instance;
                }
            }
        }

        public void Debug(string message)
        {
            handlerManager.Write(message, LogLevel.Debug);
        }

        public void Info(string message)
        {
            handlerManager.Write(message, LogLevel.Info);
        }

        public void Warning(string message)
        {
            handlerManager.Write(message, LogLevel.Warning);
        }

        public void Error(string message)
        {
            handlerManager.Write(message, LogLevel.Error);
        }

        public void Critical(string message)
        {
            handlerManager.Write(message, LogLevel.Critical);
        }
    }
}
