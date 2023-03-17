using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Executable for exit command
/// </summary>
public class ExitCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "exit";

    public override ICommandExecutable Build(IEnumerable<string> tokens, StreamSet streamSet)
    {
        return new ExitCommandExecutable(tokens, streamSet);
    }
}