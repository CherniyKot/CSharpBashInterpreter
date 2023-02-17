namespace CSharpBashInterpreter.Commands;

public interface ICommandRepresentation
{
    bool CanBeParsed(IEnumerable<string> tokens);
}