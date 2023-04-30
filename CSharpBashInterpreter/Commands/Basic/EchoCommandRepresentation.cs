using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Representation for bash echo command
/// </summary>
public class EchoCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "echo";

    public override BaseCommandExecutable Build(IEnumerable<string> tokens)
    {
        return new EchoCommandExecutable(tokens);
    }
}