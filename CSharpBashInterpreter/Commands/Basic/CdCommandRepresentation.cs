using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

public class CdCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "cd";

    public override ICommandExecutable Build(IEnumerable<string> tokens)
    {
        return new CdCommandExecutable(tokens);
    }
}
