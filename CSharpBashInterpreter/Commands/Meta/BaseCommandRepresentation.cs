using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Meta;

public abstract class BaseCommandRepresentation : ICommandRepresentation
{
    public abstract string Name { get; }
    public abstract BaseCommandExecutable Build(IEnumerable<string> tokens);
    public virtual bool CanBeParsed(IEnumerable<string> tokens) => tokens.First() == Name;
}