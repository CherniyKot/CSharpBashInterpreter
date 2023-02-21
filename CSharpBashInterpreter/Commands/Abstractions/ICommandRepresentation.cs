namespace CSharpBashInterpreter.Commands.Abstractions;

public interface ICommandRepresentation : IAbstractCommandRepresentation<IEnumerable<string>>
{
    public string Name { get; }
    BaseCommandExecutable Build(IEnumerable<string> tokens);
}