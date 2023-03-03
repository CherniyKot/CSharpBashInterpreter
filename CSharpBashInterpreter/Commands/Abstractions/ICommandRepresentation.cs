namespace CSharpBashInterpreter.Commands.Abstractions;

public interface ICommandRepresentation : IAbstractCommandRepresentation<IEnumerable<string>>
{
    public string Name { get; }
    ICommandExecutable Build(IEnumerable<string> tokens);
}