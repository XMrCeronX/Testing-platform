using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
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

        private void Connection()
        {
            string[] paths = AppDomain.CurrentDomain.BaseDirectory.Split("\\").SkipLast(4).ToArray();
            string pathToRoot = Path.Combine(paths);
            string configFilePath = Path.Combine(pathToRoot, "Config\\appsettings.json");
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFilePath, optional: false, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();

                DataTable table = conn.GetSchema("MetaDataCollections");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public MainWindowNavigationViewModel()
        {
            Login currentPage = new Login();
            ViewModel currentViewModel = new LoginViewModel();
            Context = new ApplicationContext(currentPage, currentViewModel);
            Connection();
        }
    }
}
