using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using TestingPlatform.Infrastructure.Commands;
using TestingPlatform.ViewModels.Base;
using TestingPlatform.Infrastructure.Logging;
using TestingPlatform.Infrastructure.Cryptography;
using TestingPlatform.Models;
using System.Linq;
using System.Data;
using System.Diagnostics;
using TestingPlatform.Infrastructure.Navigation;
using TestingPlatform.Views.Pages;

namespace TestingPlatform.ViewModels
{
    internal class LoginViewModel : ProgressBarViewModel
    {
        #region Login
        private string _login = "root";
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

        #region OpenLogFileCommand
        public ICommand OpenLogFileCommand { get; }
        private bool CanOpenLogFileCommand(object parameters) => true;
        private void OnOpenLogFileCommand(object parameters)
        {
            string? pathToLogFile = Logger.GetPathToLogFile();
            if (pathToLogFile == null)
            {
                Logger.Error("Не получается открыть файл логов", true);
            }
            else
            {
                new Process { StartInfo = new ProcessStartInfo(pathToLogFile) { UseShellExecute = true } }.Start();
            }
        }
        #endregion

        #region LogInCommand
        public ICommand LogInCommand { get; }
        private bool CanLogInCommand(object parameters) => true;
        private void OnLogInCommand(object parameters)
        {
            string password = (parameters as PasswordBox).Password; // немного нарушил MVVM (LoginViewModel знает о PasswordBox т.е. знает о View)
            Login = Login.Trim();
            password = password.Trim();
            if (Login == string.Empty)
            {
                Logger.Info($"Логин не должен быть пустым!", true);
                return;
            }
            if (password == string.Empty)
            {
                Logger.Info($"Пароль не должен быть пустым!", true);
                return;
            }
            string passwordHash = Cryptography.CreateMD5(password);
            Logger.Debug($"login={Login} pass={password} hash={passwordHash}");
            using (test_platformContext context = new())
            {
                User? findedUser = context.Users.Where(u => u.Login == Login && u.Password == passwordHash).FirstOrDefault();
                if (findedUser != null)
                {
                    NavigationService.Instance.NavigateToUserPage(findedUser);
                }
                else
                {
                    Logger.Info($"Пользователь {Login} не найден.", true);
                }
            }
        }
        #endregion

        public LoginViewModel()
        {
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommand, CanCloseApplicationCommand);
            LogInCommand = new LambdaCommand(OnLogInCommand, CanLogInCommand);
            OpenLogFileCommand = new LambdaCommand(OnOpenLogFileCommand, CanOpenLogFileCommand);
        }
    }
}
