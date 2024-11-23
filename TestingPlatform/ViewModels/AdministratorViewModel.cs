using System.Windows.Input;
using TestingPlatform.Infrastructure.Commands;
using TestingPlatform.Infrastructure.Navigation;
using TestingPlatform.ViewModels.Base;
using TestingPlatform.Views.Pages;

namespace TestingPlatform.ViewModels
{
    internal class AdministratorViewModel : ViewModel
    {
        #region SignOutCommand
        public ICommand SignOutCommand { get; }
        private bool CanSignOutCommand(object parameters) => true;
        private void OnSignOutCommand(object parameters)
        {
            NavigationService.Instance.Navigate(new LoginPage(new LoginViewModel()), null);
        }
        #endregion

        public AdministratorViewModel()
        {
            SignOutCommand = new LambdaCommand(OnSignOutCommand, CanSignOutCommand);
        }
    }
}
