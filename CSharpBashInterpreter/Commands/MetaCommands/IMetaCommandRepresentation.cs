using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Commands.MetaCommands;

public interface IMetaCommandRepresentation : ICommandRepresentation
{
    Task<BaseCommandExecutable> Build(IEnumerable<string> tokens, ICommandParser parser);
}