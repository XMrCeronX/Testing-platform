﻿using System;
using System.Collections.Generic;
using System.Threading;
using TestingPlatform.Infrastructure.Logging.Base;
using TestingPlatform.Infrastructure.Logging.Handlers.Base;
using static TestingPlatform.Infrastructure.Logging.Handlers.Base.Handler;

namespace TestingPlatform.Infrastructure.Logging.Handlers
{
    internal class HandlerManager
    {
        public List<Handler> Handlers { get; set; } = new List<Handler>();

        public void AddHandler(Handler handler)
        {
            Handlers.Add(handler);
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
            var dict = new Dictionary<string, StringDelegate>()
            {
                { "dateTime", () => DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff") },
                { "threadId", () => Thread.CurrentThread?.ManagedThreadId.ToString() },
                { "levelName", () => level.ToString() },
                { "message", () => message },
            };
            foreach (Handler handler in Handlers)
            {
                handler.Write(Formatter.Formatter.FormatString(dict, "dateTime [ThreadId=threadId] [levelName]: message."));
            }
        }
    }
}
