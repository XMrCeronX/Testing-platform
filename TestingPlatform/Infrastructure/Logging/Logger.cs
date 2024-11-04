using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using TestingPlatform.Infrastructure.Logging.Base;
using TestingPlatform.Infrastructure.Logging.Handlers;
using TestingPlatform.ViewModels.Base;

namespace TestingPlatform.Infrastructure.Logging
{
    internal class Logger : ILogger
    {
        private HandlerManager handlerManager { get; set; }
        public LogLevel Level { get; set; }

        public Logger()
        {
            handlerManager = new HandlerManager();
            handlerManager.AddHandler(new ConsoleHandler());
            handlerManager.AddHandler(new FileHandler(AppDomain.CurrentDomain.BaseDirectory));
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
