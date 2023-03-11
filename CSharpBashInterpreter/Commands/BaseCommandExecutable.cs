using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Meta.Utility;

namespace CSharpBashInterpreter.Commands;

/// <summary>
/// Base class with boilerplate for command executables
/// </summary>
public abstract class BaseCommandExecutable : ICommandExecutable
{
    public StreamReader InputStream { get; set; } = new(new InterruptableConsoleStream());
    public StreamWriter OutputStream { get; set; } = new(Console.OpenStandardOutput());
    public StreamWriter ErrorStream { get; set; } = new(Console.OpenStandardError());

    protected readonly string[] Tokens;

    protected BaseCommandExecutable(IEnumerable<string> tokens)
    {
        Tokens = tokens.ToArray();
    }

    public async Task<int> ExecuteAsync()
    {
        var result = await ExecuteInternalAsync();
        OutputStream.Close();
        InputStream.Close();
        ErrorStream.Close();
        return result;
    }

    protected abstract Task<int> ExecuteInternalAsync();

    public async ValueTask DisposeAsync()
    {
        InputStream.Dispose();
        await OutputStream.DisposeAsync();
        await ErrorStream.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}