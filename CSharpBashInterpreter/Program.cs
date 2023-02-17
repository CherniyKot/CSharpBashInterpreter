using CSharpBashInterpreter.Commands;
using CSharpBashInterpreter.Commands.BasicCommands;
using CSharpBashInterpreter.Commands.MetaCommands;
using CSharpBashInterpreter.Commands.MetaCommands.Utility;
using CSharpBashInterpreter.Semantics;

Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    InterruptableConsoleStream.Interrupt();
};

var tokenParser = new SpaceTokenParser();
var commandsParser = new DefaultCommandsParser
{
    Commands = new ITerminalCommandRepresentation[]{ new CatCommandRepresentation() },
    MetaCommands = new IMetaCommandRepresentation[]{ new PipeCommandRepresentation() }
};

while (true)
{
    try
    {
        var line = Console.ReadLine() ?? "";
        var tokens = tokenParser.Tokenize(line);
        var command = await commandsParser.Parse(tokens);
        await command.Execute();
    }
    catch (Exception e)
    {
        Console.Error.WriteLine(e.Message);
    }
}