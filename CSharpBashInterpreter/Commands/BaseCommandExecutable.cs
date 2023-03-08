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

    public abstract Task<int> Execute();
}