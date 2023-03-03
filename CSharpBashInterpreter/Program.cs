using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Commands.Meta;
using CSharpBashInterpreter.Commands.Meta.Utility;
using CSharpBashInterpreter.Semantics;

var context = new DefaultContext();
var tokenizer = new SpaceTokenizer();
var commandsParser = new DefaultCommandsParser(tokenizer)
{
    Commands = new ICommandRepresentation[]{ new CatCommandRepresentation(), new LsCommandRepresentation() },
    MetaCommands = new IMetaCommandRepresentation[]{ }
};

var tokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    tokenSource.Cancel();
    InterruptableConsoleStream.Interrupt();
};

var token = tokenSource.Token;
while (!token.IsCancellationRequested)
{
    try
    {
        var line = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(line))
            continue;
        var command = commandsParser.Parse(line, context);
        var result = await command.Execute();
        if (result != 0)
            Console.WriteLine($"Команда завершилась с кодом ошибки {result}.");
    }
    catch (Exception e)
    {
        Console.Error.WriteLine(e.Message);
    }
}