namespace CSharpBashInterpreter.Commands.Abstractions;

public interface ICommandRepresentation : IAbstractCommandRepresentation
{
    public string Name { get; }
    BaseCommandExecutable Build(IEnumerable<string> tokens);
}