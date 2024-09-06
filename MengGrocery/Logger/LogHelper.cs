using Microsoft.Extensions.Logging;


namespace MengGrocery.Logger
{
    public interface ILogHelper<T>
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception ex);
    }

    public class LogHelper<T> : ILogHelper<T>
    {
        private readonly ILogger<T> _logger;

        public LogHelper(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, message);
        }
    }

}