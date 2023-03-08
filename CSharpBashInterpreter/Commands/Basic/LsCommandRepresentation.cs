using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

public class LsCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "ls";
    public override ICommandExecutable Build(IEnumerable<string> tokens)
    {
        return new LsCommandExecutable(tokens);
    }
}