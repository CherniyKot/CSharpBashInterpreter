namespace CSharpBashInterpreter.Commands.BasicCommands;

public abstract class BasicTerminalCommand : AbstractTerminalCommand
{
    protected List<string> args;

    public override void Initialize(IEnumerable<string> tokens)
    {
        args = tokens.ToList();
        IsInitialized = true;
    }
}