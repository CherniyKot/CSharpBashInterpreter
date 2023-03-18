using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Executable for exit command
/// </summary>
public class ExitCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "exit";

    public override ICommandExecutable Build(IEnumerable<string> tokens)
    {
        return new ExitCommandExecutable(tokens);
    }
}