using System;
using System.Collections.Generic;
using System.Threading;
using TestingPlatform.Infrastructure.Logging.Base;
using TestingPlatform.Infrastructure.Logging.Handlers.Base;
using static TestingPlatform.Infrastructure.Logging.Formatter.Formatter;

namespace TestingPlatform.Infrastructure.Logging.Handlers
{
    internal class HandlersManager
    {
        private List<Handler> Handlers { get; set; } = new List<Handler>();

        public void AddHandler(Handler handler)
        {
            Handlers.Add(handler);
        }

        public HandlersManager AddHandlers(params Handler[] handlers)
        {
            foreach (Handler handler in handlers)
            {
                Handlers.Add(handler);
            }
            return this;
        }

        public void ClearHandlers()
        {
            Handlers.Clear();
        }

        /// <summary>
        /// Пишет message во все Handlers.
        /// </summary>
        /// <param name="message">Cообщение для вывода</param>
        /// <param name="level">Уровень лога</param>
        public void Write(string message, LogLevel level)
        {
            Dictionary<string, Func<string>> dict = new Dictionary<string, Func<string>>()
            {
                { "dateTime", () => DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff") },
                { "threadId", () => Thread.CurrentThread?.ManagedThreadId.ToString() },
                { "levelName", () => level.ToString() },
                { "message", () => message },
            };
            foreach (Handler handler in Handlers)
            {
                handler.Write(ReplaceStringsToValues(dict, "dateTime [ThreadId=threadId] [levelName]: message"));
            }
        }
    }
}
