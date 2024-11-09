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

namespace TestingPlatform.ViewModels
{
    internal class LoginViewModel : ViewModel
    {
        #region Login
        private string _login = "";
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
            Logger.Debug($"Попытка входа пользователя {Login}.");
            Logger.Debug(Login);
            Logger.Debug(password);
            string passwordHash = Cryptography.CreateMD5(password);
            Logger.Debug(passwordHash);
            using (test_platformContext context = new())
            {
                User? findedUser = context.Users.Where(u => u.Login == Login && u.Password == passwordHash).FirstOrDefault();
                if (findedUser != null)
                {
                    Logger.Debug($"{Login} вошел в систему.");
                    // GO TO USER ROLE PAGE
                }
                else
                {
                    Logger.Info($"Пользователь {Login} не найден.", true);
                }
            }
        }
        #endregion

        #region ProgressBarValue
        private int _progressBarValue = 0;
        public int ProgressBarValue
        {
            get => _progressBarValue;
            set => Set(ref _progressBarValue, value);
        }
        #endregion

        #region ProgressBarVisibility
        private Visibility _progressBarVisibility = Visibility.Collapsed; // Visibility.Collapsed
        public Visibility ProgressBarVisibility
        {
            get => _progressBarVisibility;
            set => Set(ref _progressBarVisibility, value);
        }
        #endregion

        private void ChangeProgressBarVisibility()
        {
            switch (ProgressBarVisibility)
            {
                case Visibility.Visible:
                    ProgressBarVisibility = Visibility.Collapsed; // Не отображайте элемент и не резервируйте для него место в макете.
                    break;
                case Visibility.Collapsed:
                    ProgressBarVisibility = Visibility.Visible; // Отображай элемент.
                    break;
                default:
                    ProgressBarVisibility = Visibility.Collapsed;
                    break;
            }
        }

        public LoginViewModel()
        {
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommand, CanCloseApplicationCommand);
            LogInCommand = new LambdaCommand(OnLogInCommand, CanLogInCommand);
        }
    }
}
