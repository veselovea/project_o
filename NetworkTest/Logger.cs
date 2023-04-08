using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public enum LogLevel
{
    Simple = 0,
    Base = 1,
    Advanced = 2
}

public class Logger
{
    private static Logger _instance = null;

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

    public static Logger Instance => _instance ??= new Logger(LogLevel.Simple);

    public Action<string> LogEvent { get; set; }
    public List<(string text, LogLevel level)> LogMessages => _logMessages;

    public LogLevel Level { get { return _level; } set { _level = value; } }

    public Task WriteLogMessage(string text, LogLevel level)
    {
        return Task.Run(() =>
        {
            text = $"[{DateTime.UtcNow}]\t{text}";
            if (level <= _level)
                LogEvent?.Invoke(text);
            _logMessages.Add((text, level));
        });
    }

    public void Destroy()
    {
        SaveLog();
        _timer.Stop();
        _timer = null;
    }

    private void SaveLog()
    {
        using (StreamWriter writer = new StreamWriter("log.txt"))
            writer.WriteLine(string.Join('\n', _logMessages));
    }
}