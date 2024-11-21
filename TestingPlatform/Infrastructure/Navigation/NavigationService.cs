using System;
using System.Linq;
using System.Windows.Controls;
using TestingPlatform.Infrastructure.Logging;
using TestingPlatform.Models;
using TestingPlatform.ViewModels;
using TestingPlatform.Views.Pages;

namespace TestingPlatform.Infrastructure.Navigation
{
    public class NavigationService
    {
        public class SecondInitFrameException : Exception
        {
            public SecondInitFrameException(string message) : base(message) { }
        }

        public class NoPageForCurrentUserException : Exception
        {
            public NoPageForCurrentUserException(string message) : base(message) { }
        }

        public class RoleIdIsNotExistException : Exception
        {
            public RoleIdIsNotExistException(string message) : base(message) { }
        }

        public static readonly NavigationService Instance = new NavigationService();
        private NavigationService() { }

        #region Frame
        private Frame? _frame;
        public Frame Frame
        {
            get => _frame ?? throw new NullReferenceException("Frame не определён.");
            set
            {
                if (_frame == null)
                {
                    _frame = value;
                }
                else
                {
                    throw new SecondInitFrameException("Повторное присвоение Frame.");
                }
            }
        }
        #endregion

        #region User
        private User? _user;
        public User User
        {
            get => _user ?? throw new NullReferenceException("User не определён.");
            set => _user = value;
        }
        #endregion

        public void GoBack()
        {
            Frame.GoBack();
        }

        public void NavigateToUserPage(User user)
        {
            Role? role = null;
            using (test_platformContext context = new())
            {
                role = context.Roles.Where(r => r.Id == user.RoleId).First();
            }
            if (role != null)
            {
                switch (role.Name)
                {
                    case "Администратор":
                        Navigate(new AdministratorPage(new AdministratorViewModel()), user);
                        break;
                    default:
                        string err = $"Не существует старницы перехода для роли - {role.Name}.";
                        Logger.Critical(err, true);
                        //throw new NoPageForCurrentUserException(err);
                        break;
                }
            }
        }

        public void Navigate(Page page)
        {
            Logger.Debug($"Переход на страницу {page}.");
            Frame.Navigate(page);
        }

        public void Navigate(Page page, User? user)
        {
            if (user == null && _user != null)
            {
                Logger.Info($"Пользователь {_user.Name} вышел из системы.");
            }
            if (user != null && _user == null)
            {
                Logger.Debug($"Пользователь {user.Name} вошёл в систему.");
            }
            _user = user;
            Navigate(page);
        }
    }
}
