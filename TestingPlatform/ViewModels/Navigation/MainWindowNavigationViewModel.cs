﻿using System.Windows.Controls;
using TestingPlatform.ViewModels.Base;
using TestingPlatform.Views.Pages;

namespace TestingPlatform.ViewModels.Navigation
{
    internal class MainWindowNavigationViewModel : ViewModel
    {
        #region CurrentPage
        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set => Set(ref _currentPage, value);
        }
        #endregion

        #region CurrentViewModel
        private ViewModel _currentViewModel;
        public ViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => Set(ref _currentViewModel, value);
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

        private void Login()
        {
            CurrentPage = new Login();
            CurrentViewModel = new LoginViewModel();
        }

        public MainWindowNavigationViewModel()
        {
            Login();
        }
    }
}