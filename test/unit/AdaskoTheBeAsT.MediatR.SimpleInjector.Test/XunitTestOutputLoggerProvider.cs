using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public sealed class XunitTestOutputLoggerProvider(
    ITestOutputHelper output,
    Func<string, LogLevel, bool>? filter = null)
    : ILoggerProvider
{
    private readonly ITestOutputHelper _output = output ?? throw new ArgumentNullException(nameof(output));

    public ILogger CreateLogger(string categoryName) =>
        new XunitTestOutputLogger(_output, categoryName, filter);

    public void Dispose()
    {
        // noop
    }
}
