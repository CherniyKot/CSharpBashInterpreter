using CSharpBashInterpreter.Commands;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Commands.Meta.Utility;
using CSharpBashInterpreter.Semantics;

var context = new DefaultContext();
var tokenizer = new SpaceTokenizer();
var commandsParser = new DefaultCommandsParser()
{
    Commands = new ICommandRepresentation[]{ 
        new CatCommandRepresentation(),
        new LsCommandRepresentation(),
        new EchoCommandRepresentation(),
        new PwdCommandRepresentation(),
        new WcCommandRepresentation(),
        new ExitCommandRepresentation()
    },
    MetaCommands = new IMetaCommandRepresentation[]{ },
    ExternalCommandRepresentation = new ExternalCommandRepresentation()
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
        var tokens = tokenizer.Tokenize(line);
        if (tokens.Length == 0)
            continue;
        var command = commandsParser.Parse(tokens, context);
        var result = await command.Execute();
        if (result != 0)
            Console.WriteLine($"Команда завершилась с кодом ошибки {result}.");
    }
    catch (Exception e)
    {
        Console.Error.WriteLine(e.Message);
    }
}
