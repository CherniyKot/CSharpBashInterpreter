namespace CSharpBashInterpreter.Commands.Basic;

public class CatCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "cat";

    public override BaseCommandExecutable Build(IEnumerable<string> tokens)
    {
        return new CatCommandExecutable(tokens);
    }
}