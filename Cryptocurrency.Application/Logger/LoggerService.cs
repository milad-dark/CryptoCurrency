using Microsoft.Extensions.Logging;

namespace Cryptocurrency.Application.Logger;

public class LoggerService<T> : ILoggerService<T>
{
    private readonly ILogger<T> _logger;

    public LoggerService(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message) => _logger.LogInformation(message);
    public void LogWarning(string message) => _logger.LogWarning(message);
    public void LogError(string message, Exception ex = null)
    {
        if (ex != null)
            _logger.LogError(ex, message);
        else
            _logger.LogError(message);
    }
    public void LogDebug(string message) => _logger.LogDebug(message);
    public void LogCritical(string message, Exception ex = null)
    {
        if (ex != null)
            _logger.LogCritical(ex, message);
        else
            _logger.LogCritical(message);
    }
}
