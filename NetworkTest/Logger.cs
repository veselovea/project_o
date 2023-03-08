using System;

public enum LogLevel
{
    Simple = 0,
    Base = 1,
    Advanced = 2
}

public class Logger
{
    private LogLevel _level;

    public Logger(LogLevel level)
    {
        _level = level;
    }

    public Action<string> LogEvent { get; set; }

    public LogLevel Level { get { return _level; } set { _level = value; } }

    public void PrintLog(string text, LogLevel level)
    {
        if (level <= _level)
            LogEvent?.Invoke(text);
    }
}

