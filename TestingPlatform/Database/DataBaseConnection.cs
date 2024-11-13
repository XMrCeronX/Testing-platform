using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace TestingPlatform.Database
{
    internal class DataBaseConnection
    {
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
