namespace CSharpBashInterpreter.Commands.Factory;

public class CommandFactory<T> : ICommandFactory<T> where T : AbstractTerminalCommand, IParsable, new()
{
    public T Instantiate(IEnumerable<string> tokens)
    {
        var t = new T();
        t.Initialize(tokens);
        return t;
    }

    public bool CanBeParsed(IEnumerable<string> tokens)
    {
        return T.CanBeParsed(tokens);
    }
}