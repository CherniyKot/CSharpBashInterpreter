using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

public abstract class BaseCommandRepresentation : ICommandRepresentation
{
    public abstract string Name { get; }
    public abstract ICommandExecutable Build(IEnumerable<string> tokens);
    public virtual bool CanBeParsed(IEnumerable<string> tokens) => tokens.First() == Name;
}