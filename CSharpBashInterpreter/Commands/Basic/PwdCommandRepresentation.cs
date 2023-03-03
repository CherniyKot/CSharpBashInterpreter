namespace CSharpBashInterpreter.Commands.Basic;

public class PwdCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "pwd";

    public override BaseCommandExecutable Build(IEnumerable<string> tokens)
    {
        return new PwdCommandExecutable(tokens);
    }
}
