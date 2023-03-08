using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

public class CatCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "cat";

    public override ICommandExecutable Build(IEnumerable<string> tokens)
    {
        return new CatCommandExecutable(tokens);
    }
}