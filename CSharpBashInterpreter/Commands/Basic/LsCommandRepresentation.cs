using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Representation for bash ls command
/// </summary>
internal class LsCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "ls";

    public override ICommandExecutable Build(IEnumerable<string> tokens)
    {
        return new LsCommandExecutable(tokens);
    }
}
