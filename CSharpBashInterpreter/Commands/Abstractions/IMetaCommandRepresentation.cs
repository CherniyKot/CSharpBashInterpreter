using CSharpBashInterpreter.Semantics.Abstractions;

namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
/// Provide information of meta command, who process input and provide additional logic
/// </summary>
public interface IMetaCommandRepresentation : IAbstractCommandRepresentation
{
    /// <summary>
    /// Build combined executable with custom processed string.
    /// </summary>
    /// <param name="input">Console input string</param>
    /// <param name="context">Environment context</param>
    /// <param name="parser">Parsed for additional command parsing</param>
    ICommandExecutable Build(IEnumerable<string> input, IContext context, ICommandParser parser);
}