using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Commands.Meta;
using CSharpBashInterpreter.Commands.Meta.Utility;
using CSharpBashInterpreter.Semantics;

Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    InterruptableConsoleStream.Interrupt();
};

var context = new DefaultContext();
var tokenParser = new SpaceTokenParser();
var commandsParser = new DefaultCommandsParser
{
    Commands = new ICommandRepresentation[]{ new CatCommandRepresentation() },
    MetaCommands = new IMetaCommandRepresentation[]{ new PipeCommandRepresentation() }
};

while (true)
{
    try
    {
        var line = Console.ReadLine() ?? "";
        var tokens = tokenParser.Tokenize(line);
        var command = commandsParser.Parse(tokens, context);
        await command.Execute();
    }
    catch (Exception e)
    {
        Console.Error.WriteLine(e.Message);
    }
}