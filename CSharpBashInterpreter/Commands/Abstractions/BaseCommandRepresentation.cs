namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
/// Base class for representations of general interpreter commands.
/// </summary>
public abstract class BaseCommandRepresentation : ICommandRepresentation
{
    public abstract string Name { get; }
    public abstract ICommandExecutable Build(IEnumerable<string> tokens);

    public virtual bool CanBeParsed(IEnumerable<string> tokens) =>
        tokens.First() == Name;
}