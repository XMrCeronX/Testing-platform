using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Windows.Controls;
using TestingPlatform.ViewModels.Base;
using System.Linq;

namespace TestingPlatform.Infrastructure.Context
{
    internal class ApplicationContext
    {

        public Page CurrentPage { get; set; }
        public ViewModel CurrentViewModel { get; set; }


        public ApplicationContext(Page currentPage, ViewModel currentViewModel)
        {
            CurrentPage = currentPage;
            CurrentViewModel = currentViewModel;
        }

        public static string GetConnectionString()
        {
            string[] paths = AppDomain.CurrentDomain.BaseDirectory.Split("\\").SkipLast(4).ToArray();
            string pathToRoot = Path.Combine(paths);
            string configFilePath = Path.Combine(pathToRoot, "Config\\appsettings.json");
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFilePath, optional: false, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            return connectionString;
        }
    }
}
