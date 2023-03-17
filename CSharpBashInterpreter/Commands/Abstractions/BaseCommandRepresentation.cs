using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

public abstract class BaseCommandRepresentation : ICommandRepresentation
{
    public abstract string Name { get; }
    public abstract ICommandExecutable Build(IEnumerable<string> tokens, StreamSet streamSet);

    public virtual bool CanBeParsed(IEnumerable<string> tokens)
    {
        return tokens.First() == Name;
    }
}