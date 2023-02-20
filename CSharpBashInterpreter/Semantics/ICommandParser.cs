using CSharpBashInterpreter.Commands;

namespace CSharpBashInterpreter.Semantics;

public interface ICommandParser
{
    BaseCommandExecutable Parse(IEnumerable<string> tokens, IContext context);
}