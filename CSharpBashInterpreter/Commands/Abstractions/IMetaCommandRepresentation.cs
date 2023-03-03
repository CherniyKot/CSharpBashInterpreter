using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Commands.Abstractions;

public interface IMetaCommandRepresentation : IAbstractCommandRepresentation<IEnumerable<string>>
{
    ICommandExecutable Build(string[] input, IContext context, ICommandParser parser);
}