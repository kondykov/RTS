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
        CANCELED,
        WARNING,
        CRITICAL_ERROR,
    }
    public interface ILoggerService
    {
        public string Read();
        public void Write(string message);
        protected string CollectMessage<T>(string message, T obj, LogStatus logStatus, Exception ex = null);
    }
}
