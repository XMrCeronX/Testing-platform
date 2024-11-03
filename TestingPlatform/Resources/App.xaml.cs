using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
            App app = new App();
            app.InitializeComponent(); // Загружает ресурсы из App.xaml
            app.Run(new MainWindow()); // Запускает указанное окно
        }
    }
}
