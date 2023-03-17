using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Representation for bash wc command
/// </summary>
public class WcCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "wc";

    public override ICommandExecutable Build(IEnumerable<string> tokens, StreamSet streamSet)
    {
        return new WcCommandExecutable(tokens, streamSet);
    }
}