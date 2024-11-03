using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Test_platform.Infrastructure.Commands;
using Test_platform.ViewModels.Base;

namespace Test_platform.ViewModels
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
            //ChangeProgressBarVisibility();
            string password = (parameters as PasswordBox).Password; // немного нарушил MVVM (LoginViewModel знает о PasswordBox т.е. знает о View)
            Console.WriteLine(Login);
            Console.WriteLine(password);
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

        Random rand = new Random();
        DispatcherTimer dispatcherTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(5) // Интервал срабатывания таймера
        };

        private void CreateTaskProgressBarValueUpdate()
        {
            dispatcherTimer.Tick += (object sender, EventArgs e) =>
            {
                int newMumber = rand.Next(1, 100);
                ProgressBarValue = newMumber;
            };
            dispatcherTimer.Start();
        }

        public LoginViewModel()
        {
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommand, CanCloseApplicationCommand);
            LogInCommand = new LambdaCommand(OnLogInCommand, CanLogInCommand);
            //CreateTaskProgressBarValueUpdate(); // обновление значения ProgressBar
        }

        ~LoginViewModel()
        {
            dispatcherTimer.Stop();
        }
    }
}
