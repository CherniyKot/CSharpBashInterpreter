using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Semantics;

/// <summary>
/// Provide logic of parsing tokens to specific commands
/// </summary>
public interface ICommandParser
{
    /// <summary>
    /// Use tokens to match command and create command executable
    /// </summary>
    /// <param name="input">Tokenized input string</param>
    /// <param name="context"></param>
    ICommandExecutable Parse(string[] input, IContext context);
}