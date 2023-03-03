using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Semantics;

public interface ICommandParser
{
    ICommandExecutable Parse(string[] input, IContext context);
}