using LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public class Logger : ILoggerService
    {
        protected TimeOnly _time = new();
        public Logger() { }
        async public void Write(string message)
        {

        }
        public string Read()
        {
            return null;
        }
        public string CollectMessage<T>(string message, T obj, LogStatus logStatus, Exception ex = null) => ex == null ? 
            $"{_time}|{message}|{typeof(T).Namespace}.{nameof(obj)}|{logStatus}" :
            $"{_time}|{message}|{typeof(T).Namespace}.{nameof(obj)}|{logStatus}|{ex.Message}";
    }
}
