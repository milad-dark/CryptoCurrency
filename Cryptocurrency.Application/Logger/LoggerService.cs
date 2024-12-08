using Serilog;

namespace Cryptocurrency.Application.Logger
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger _logger;

        public LoggerService()
        {
            _logger = Log.Logger;
        }

        public void LogInformation(string message) => _logger.Information(message);
        public void LogWarning(string message) => _logger.Warning(message);
        public void LogError(string message, Exception ex = null)
        {
            if (ex != null)
                _logger.Error(ex, message);
            else
                _logger.Error(message);
        }
        public void LogDebug(string message) => _logger.Debug(message);
        public void LogCritical(string message, Exception ex = null)
        {
            if (ex != null)
                _logger.Fatal(ex, message);
            else
                _logger.Fatal(message);
        }
    }
}
