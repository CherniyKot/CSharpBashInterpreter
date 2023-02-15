namespace CSharpBashInterpreter.Commands.Factory;

public interface ICommandFactory<out T> where T : AbstractTerminalCommand
{
    public T Instantiate(IEnumerable<string> tokens);

    public bool CanBeParsed(IEnumerable<string> tokens);
}