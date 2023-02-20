namespace CSharpBashInterpreter.Commands.BasicCommands;

public class CatCommandRepresentation : ITerminalCommandRepresentation
{
    public string Name => "cat";

    public bool CanBeParsed(IEnumerable<string> tokens)
    {
        return tokens.First() == Name;
    }

    public async Task<BaseCommandExecutable> Build(IEnumerable<string> tokens)
    {
        var command = new CatCommandExecutable();
        await command.Initialize(tokens);
        return command;
    }
}