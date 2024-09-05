namespace AluraRPA.Infrastructure.Extensions;
public static partial class LoggingExtensions
{
    [LoggerMessage(EventId = 1,
                Level = Microsoft.Extensions.Logging.LogLevel.Information,
                Message = "{message}")]
    public static partial void LogBusiness(this ILogger logger, string? message);

    [LoggerMessage(EventId = 2,
                Level = Microsoft.Extensions.Logging.LogLevel.Warning,
                Message = "{message}")]
    public static partial void LogInfra(this ILogger logger, string? message);
}