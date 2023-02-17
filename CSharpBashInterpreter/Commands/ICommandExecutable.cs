namespace CSharpBashInterpreter.Commands;

public interface ICommandExecutable
{
    Task Initialize(IEnumerable<string> tokens);
    Task Execute();
}