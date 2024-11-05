using System.Windows.Controls;
using TestingPlatform.Infrastructure.Context;
using TestingPlatform.ViewModels.Base;
using TestingPlatform.Views.Pages;

namespace TestingPlatform.ViewModels.Navigation
{
    internal class MainWindowNavigationViewModel : ViewModel
    {
        private ApplicationContext Context { get; set; }

        #region CurrentPage
        public Page CurrentPage
        {
            get => Context.CurrentPage;
        }
        #endregion

        #region Title
        private string _tiile = "Платформа для тестирования";
        public string Title
        {
            get => _tiile;
            set => Set(ref _tiile, value);
        }
        #endregion

        public MainWindowNavigationViewModel()
        {
            Login currentPage = new Login();
            ViewModel currentViewModel = new LoginViewModel();
            Context = new ApplicationContext(currentPage, currentViewModel);
        }
    }
}
