using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TestingPlatform.Infrastructure.Logging;
using TestingPlatform.Infrastructure.Navigation;
using TestingPlatform.ViewModels.Base;
using TestingPlatform.Views.Pages;

namespace TestingPlatform.ViewModels.Navigation
{
    internal class MainWindowNavigationViewModel : ViewModel
    {
        #region Title
        private string _tiile = "Платформа для тестирования";
        public string Title
        {
            get => _tiile;
            set => Set(ref _tiile, value);
        }
        #endregion

        #region ContentFrame
        private Frame _contentFrame = new();
        public Frame ContentFrame
        {
            get => _contentFrame;
            set => Set(ref _contentFrame, value);
        }
        #endregion

        private void MainWindowClosingEvent(object sender, CancelEventArgs e)
        {
            Logger.Debug($"Закрытие приложения.");
        }

        public MainWindowNavigationViewModel()
        {
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindowClosingEvent);
            NavigationService.Instance.Frame = ContentFrame;
            NavigationService.Instance.Navigate(new LoginPage(new LoginViewModel()));
        }
    }
}
