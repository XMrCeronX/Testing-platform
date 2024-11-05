using System;
using System.Windows;
using TestingPlatform.Infrastructure.Logging.Base;
using TestingPlatform.Infrastructure.Logging.Handlers;

namespace TestingPlatform.Infrastructure.Logging
{
    public class Logger
    {
        private static HandlersManager handlerManager = new HandlersManager().AddHandlers(
                new ConsoleHandler(),
                new FileHandler(AppDomain.CurrentDomain.BaseDirectory)
        );

        public static void Debug(string message, bool usingMessageBox = false)
        {
            handlerManager.Write(message, LogLevel.Debug);
            if (usingMessageBox) MessageBox.Show(message);
        }

        public static void Info(string message)
        {
            handlerManager.Write(message, LogLevel.Info);
        }

        public static void Warning(string message)
        {
            handlerManager.Write(message, LogLevel.Warning);
        }

        public static void Error(string message)
        {
            handlerManager.Write(message, LogLevel.Error);
        }

        public static void Critical(string message)
        {
            handlerManager.Write(message, LogLevel.Critical);
        }
    }
}
