using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public enum LogLevel
{
    Simple = 0,
    Base = 1,
    Advanced = 2
}

public class Logger
{
#warning Добавить возможность экспорта логов в файл
    private LogLevel _level;
    private List<(string text, LogLevel level)> _logMessages;
    private Stopwatch _timer;

    public Logger(LogLevel level)
    {
        _level = level;
        _logMessages = new List<(string text, LogLevel level)>();
        _timer = new Stopwatch();
        _timer.Start();
    }

    ~Logger()
    {
        _timer.Stop();
        _timer = null;
    }

    public Action<string>? LogEvent { get; set; }
    public List<(string text, LogLevel level)> LogMessages => _logMessages;

    public LogLevel Level { get { return _level; } set { _level = value; } }

    public Task WriteLogMessage(string text, LogLevel level)
    {
        text = $"[{DateTime.UtcNow}]\t{text}";
        if (level <= _level)
            LogEvent?.Invoke(text);
        _logMessages.Add((text, level));
        return Task.CompletedTask;
    }
}


