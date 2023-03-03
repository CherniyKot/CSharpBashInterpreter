namespace CSharpBashInterpreter.Commands.Basic;

class EchoCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "echo";
    public override BaseCommandExecutable Build(IEnumerable<string> tokens)
    {
        return new EchoCommandExecutable(tokens);
    }
}
