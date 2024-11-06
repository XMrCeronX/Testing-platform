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

        public static void Debug(string message, bool usingMessageBox = false, string caption = "Сообщение отладки")
        {
            handlerManager.Write(message, LogLevel.Debug);
            if (usingMessageBox) MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static void Info(string message, bool usingMessageBox = false, string caption = "Сообщение")
        {
            handlerManager.Write(message, LogLevel.Info);
            if (usingMessageBox) MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void Warning(string message, bool usingMessageBox = false, string caption = "Предупреждение")
        {
            handlerManager.Write(message, LogLevel.Warning);
            if (usingMessageBox) MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void Error(string message, bool usingMessageBox = false, string caption = "Ошибка")
        {
            handlerManager.Write(message, LogLevel.Error);
            if (usingMessageBox) MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Critical(string message, bool usingMessageBox = false, string caption = "Критическая ошибка")
        {
            handlerManager.Write(message, LogLevel.Critical);
            if (usingMessageBox) MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
