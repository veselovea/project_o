using System;
using System.Collections.Generic;

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

    public Logger(LogLevel level)
    {
        _level = level;
        _logMessages = new List<(string text, LogLevel level)>();
    }

    public Action<string>? LogEvent { get; set; }
    public List<(string text, LogLevel level)> LogMessages => _logMessages;

    public LogLevel Level { get { return _level; } set { _level = value; } }

    public void WriteLogMessage(string text, LogLevel level)
    {
        if (level <= _level)
            LogEvent?.Invoke(text);
        _logMessages.Add((text, level));
    }
}

