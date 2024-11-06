using System;
using System.IO;
using TestingPlatform.Infrastructure.Logging.Handlers.Base;

namespace TestingPlatform.Infrastructure.Logging.Handlers
{
    internal class FileHandler : Handler
    {
        public string PathToDir { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }

        public FileHandler(string pathToDir, string fileName = "log.log")
        {
            PathToDir = pathToDir;
            FileName = fileName;
            FullPath = Path.Combine(PathToDir, FileName);
            CreateLogFileIfNotExists();
        }

        private void CreateLogFileIfNotExists()
        {
            if (!File.Exists(FullPath))
            {
                try
                {
                    using (StreamWriter w = File.AppendText(FullPath)) { }; // создание пустого файла
                }
                catch (Exception)
                {
                    Logger.Error($"Ошибка создания файла для логов: {FullPath}.", true);   
                }
            }
        }

        public override async void Write(string message)
        {
            using (StreamWriter writer = new StreamWriter(FullPath, true))
            {
                await writer.WriteLineAsync(message);
            }
        }
    }
}
