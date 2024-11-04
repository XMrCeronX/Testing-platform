using System.Collections.Generic;

namespace TestingPlatform.Infrastructure.Logging.Handlers.Base
{
    internal abstract class Handler
    {
        public delegate string StringDelegate();
        public abstract void Write(string message);
    }
}
