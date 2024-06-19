using System;

namespace LoggerService;

public class Logger<T>(FileService fileService) : ILoggerService<T>
{
    private readonly string _time = DateTime.Now.ToLongTimeString();
    private readonly string _namespace = typeof(T).FullName;

    public string CollectMessage(string message, LogStatus logStatus, Exception ex = null) => ex == null
        ? $"{_time} | {message} | {_namespace} | {logStatus}"
        : $"{_time} | {message} | {_namespace} | {logStatus} | {ex.Message}";

    public void Log(LogStatus logStatus, string message, Exception ex = null)
    {
        var response = CollectMessage(message, logStatus, ex);
        switch (logStatus)
        {
            case LogStatus.Ok:
                LogInformation(response);
                break;
            case LogStatus.Warning:
                LogWarning(response);
                break;
            case LogStatus.Error:
                LogError(response, ex);
                break;
            case LogStatus.CriticalError:
                LogCritical(response, ex);
                break;
        }
    }

#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
    public void LogInformation(string message) => fileService.WriteLog(message);
    public void LogWarning(string message) => fileService.WriteLog(message);
    public void LogError(string message, Exception exception) => fileService.WriteLog(message);
    public void LogCritical(string message, Exception exception) => fileService.WriteLog(message);
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
}