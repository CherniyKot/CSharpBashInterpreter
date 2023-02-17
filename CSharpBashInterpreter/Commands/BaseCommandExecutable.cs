using CSharpBashInterpreter.Commands.MetaCommands.Utility;

namespace CSharpBashInterpreter.Commands;

public abstract class BaseCommandExecutable : ICommandExecutable
{
    public StreamReader InputStream { get; set; } = new(new InterruptableConsoleStream());
    public StreamWriter OutputStream { get; set; } = new(Console.OpenStandardOutput());
    public StreamWriter ErrorStream { get; set; } = new(Console.OpenStandardError());

    protected string[] Tokens { get; private set; } = null!;

    public virtual Task Initialize(IEnumerable<string> tokens)
    {
        Tokens = tokens.ToArray();
        return Task.CompletedTask;
    }
    
    public abstract Task Execute();
}