using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Representation for bash pwd command
/// </summary>
public class PwdCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "pwd";

    public override BaseCommandExecutable Build(IEnumerable<string> tokens)
    {
        return new PwdCommandExecutable(tokens);
    }
}