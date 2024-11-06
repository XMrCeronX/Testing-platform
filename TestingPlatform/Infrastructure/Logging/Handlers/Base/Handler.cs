namespace TestingPlatform.Infrastructure.Logging.Handlers.Base
{
    internal abstract class Handler
    {
        public abstract void Write(string message);
    }
}
