using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
///     Provide information and logic to call external command from console
/// </summary>
public interface IExternalCommandRepresentation : IAbstractCommandRepresentation
{
    /// <summary>
    ///     Build executable for external invoke command.
    /// </summary>
    /// <param name="tokens">Console input tokens</param>
    /// <param name="context">Environment context</param>
    /// <param name="streamSet">Stream set for command</param>
    /// <returns></returns>
    ICommandExecutable Build(IEnumerable<string> tokens, IContext context, StreamSet streamSet);
}