using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Representation for bash echo command
/// </summary>
public class EchoCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "echo";

    public override BaseCommandExecutable Build(IEnumerable<string> tokens, StreamSet streamSet)
    {
        return new EchoCommandExecutable(tokens, streamSet);
    }
}