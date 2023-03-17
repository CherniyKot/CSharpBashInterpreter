using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Representation for bash pwd command
/// </summary>
public class PwdCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "pwd";

    public override BaseCommandExecutable Build(IEnumerable<string> tokens, StreamSet streamSet)
    {
        return new PwdCommandExecutable(tokens, streamSet);
    }
}