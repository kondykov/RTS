using LoggerService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public class Logger<T> : ILoggerService<T>
    {
        protected string _time = DateTime.Now.ToLongTimeString();
        private readonly FileService _fileService;
        private readonly string _namespace;

        public Logger(FileService fileService)
        {
            _fileService = fileService;
            _namespace = typeof(T).FullName;
        }

        public string CollectMessage(string message, LogStatus logStatus, Exception ex = null) => ex == null
            ? $"{_time} | {message} | {_namespace} | {logStatus}"
            : $"{_time} | {message} | {_namespace} | {logStatus} | {ex.Message}";

        public void Log(LogStatus logStatus, string message, Exception ex = null)
        {
            string response = CollectMessage(message, logStatus, ex);
            switch (logStatus)
            {
                case LogStatus.OK:
                    LogInformation(response);
                    break;
                case LogStatus.WARNING:
                    LogWarning(response);
                    break;
                case LogStatus.ERROR:
                    LogError(response, ex);
                    break;
                case LogStatus.CRITICAL_ERROR:
                    LogCritical(response, ex);
                    break;
                default:
                    break;
            }
        }

#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
        public void LogInformation(string message) => _fileService.WriteLog(message);
        public void LogWarning(string message) => _fileService.WriteLog(message);
        public void LogError(string message, Exception exception) => _fileService.WriteLog(message);
        public void LogCritical(string message, Exception exception) => _fileService.WriteLog(message);
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
    }
}