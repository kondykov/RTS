using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public enum LogStatus
    {
        OK,
        WARNING,
        ERROR,
        CRITICAL_ERROR,
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
