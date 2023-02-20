namespace CSharpBashInterpreter.Commands.Abstractions;

public interface IAbstractCommandRepresentation
{
    bool CanBeParsed(IEnumerable<string> tokens);
}