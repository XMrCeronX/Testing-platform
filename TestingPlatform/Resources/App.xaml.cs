using System;
using System.Windows;
using TestingPlatform.Infrastructure.Logging;

namespace TestingPlatform
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread] // Обязательно для WPF приложений
        public static void Main()
        {
            Logger.Debug("Запуск приложения.");
            App app = new App();
            app.InitializeComponent(); // Загружает ресурсы из App.xaml
            app.Run(new MainWindow()); // Запускает указанное окно
        }
    }
}
