using System.Diagnostics;
using TestingPlatform.Infrastructure.Logging.Handlers.Base;

namespace TestingPlatform.Infrastructure.Logging.Handlers
{
    internal class ConsoleHandler : Handler
    { 
        public ConsoleHandler() { }

        public override void Write(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
