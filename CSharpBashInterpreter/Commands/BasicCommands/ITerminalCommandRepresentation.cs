namespace CSharpBashInterpreter.Commands.BasicCommands;

public interface ITerminalCommandRepresentation : ICommandRepresentation
{
    public string Name { get; }
    Task<BaseCommandExecutable> Build(IEnumerable<string> tokens);
}