using CSharpBashInterpreter.Commands;

namespace CSharpBashInterpreter.Semantics;

public interface ICommandParser
{
    Task<BaseCommandExecutable> Parse(IEnumerable<string> tokens);
}