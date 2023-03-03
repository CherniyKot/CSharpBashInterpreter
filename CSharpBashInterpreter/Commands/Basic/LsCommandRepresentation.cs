namespace CSharpBashInterpreter.Commands.Basic;

public class LsCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "ls";
    public override BaseCommandExecutable Build(IEnumerable<string> tokens)
    {
        return new LsCommandExecutable(tokens);
    }
}