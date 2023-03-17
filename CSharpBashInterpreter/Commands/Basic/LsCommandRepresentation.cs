using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Representation for bash ls command
/// </summary>
internal class LsCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "ls";

    public override ICommandExecutable Build(IEnumerable<string> tokens, StreamSet streamSet)
    {
        return new LsCommandExecutable(tokens, streamSet);
    }
}