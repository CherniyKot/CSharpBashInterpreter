using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Representation for bash wc command
/// </summary>
public class WcCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "wc";

    public override ICommandExecutable Build(IEnumerable<string> tokens) =>
        new WcCommandExecutable(tokens);
}