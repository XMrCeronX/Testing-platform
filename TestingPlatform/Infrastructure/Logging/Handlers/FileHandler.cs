using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
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
                    using (StreamWriter w = File.AppendText(FullPath)) { }; // создание поустого файла
                }
                catch (Exception)
                {
                    MessageBox.Show($"Ошибка создания файла \'{FullPath}\'.");   
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
