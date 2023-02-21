namespace CSharpBashInterpreter.Commands.Abstractions;

public interface IAbstractCommandRepresentation<in TInput>
{
    bool CanBeParsed(TInput data);
}