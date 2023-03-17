using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Representation for bash cat command
/// </summary>
public class CatCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "cat";

    public override ICommandExecutable Build(IEnumerable<string> tokens, StreamSet streamSet)
    {
        return new CatCommandExecutable(tokens, streamSet);
    }
}