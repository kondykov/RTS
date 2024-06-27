using System;

namespace LoggerService
{
    public enum LogStatus
    {
        Ok,
        Warning,
        Error,
        CriticalError,
    }

    public interface ILoggerService<T>
    {
        public string CollectMessage(string message, LogStatus logStatus, Exception ex = null);
        public void LogInformation(string message);
        public void LogWarning(string message);
        public void LogError(string message, Exception ex);
        public void LogCritical(string message, Exception ex);
    }
}