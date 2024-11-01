using System.Windows;
using System.Windows.Input;
using Test_platform.Infrastructure.Commands;
using Test_platform.ViewModels.Base;

namespace Test_platform.ViewModels
{
    internal class LoginViewModel : ViewModel
    {
        #region Login
        private string _login = "John";
        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }
        #endregion

        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommand(object parameters) => true;
        private void OnCloseApplicationCommand(object parameters)
        {
            Application.Current.Shutdown();
        }
        #endregion

        public LoginViewModel()
        {
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommand, CanCloseApplicationCommand);

        }
    }
}
