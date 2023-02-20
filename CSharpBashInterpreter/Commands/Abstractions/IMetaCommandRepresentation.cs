using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Commands.Abstractions;

public interface IMetaCommandRepresentation : IAbstractCommandRepresentation
{
    BaseCommandExecutable Build(IEnumerable<string> tokens, IContext context, ICommandParser parser);
}