using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Representation for bash cat command
/// </summary>
public class CatCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "cat";

    public override ICommandExecutable Build(IEnumerable<string> tokens) =>
        new CatCommandExecutable(tokens);
}