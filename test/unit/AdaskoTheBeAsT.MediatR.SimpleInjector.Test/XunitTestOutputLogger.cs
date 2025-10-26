using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public sealed class XunitTestOutputLogger(
    ITestOutputHelper output,
    string category,
    Func<string, LogLevel, bool>? filter)
    : ILogger
{
    private readonly LoggerExternalScopeProvider _scopes = new();

    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull =>
        _scopes.Push(state);

    public bool IsEnabled(LogLevel logLevel) =>
        filter?.Invoke(category, logLevel) ?? logLevel != LogLevel.None;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter?.Invoke(state, exception);
        try
        {
            output.WriteLine($"{DateTimeOffset.Now:O} [{logLevel}] {category}: {message}");
            if (exception != null)
            {
                output.WriteLine(exception.ToString());
            }
        }
#pragma warning disable CC0004
        catch (InvalidOperationException)
        {
            // ITestOutputHelper might be invalid outside the test's lifetime; ignore.
        }
#pragma warning restore CC0004
    }
}
