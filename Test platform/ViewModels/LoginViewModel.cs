using System.Windows;
using System.Windows.Controls;
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

        #region LogInCommand
        public ICommand LogInCommand { get; }
        private bool CanLogInCommand(object parameters) => true;
        private void OnLogInCommand(object parameters)
        {
            string password = (parameters as PasswordBox).Password; // немного нарушил MVVM (LoginViewModel знает о PasswordBox т.е. знает о View)
            MessageBox.Show($"{_login}\n{password}");
        }
        #endregion

        public LoginViewModel()
        {
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommand, CanCloseApplicationCommand);
            LogInCommand = new LambdaCommand(OnLogInCommand, CanLogInCommand);
        }
    }
}
