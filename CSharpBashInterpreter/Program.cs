using CSharpBashInterpreter;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Commands.External;
using CSharpBashInterpreter.Commands.Meta;
using CSharpBashInterpreter.Semantics.Context;
using CSharpBashInterpreter.Semantics.Parsing;

var tokenizer = new SpaceTokenizer();
var commandsParser = new DefaultCommandsParser
{
    Commands = new ICommandRepresentation[]
    {
        new CatCommandRepresentation(),
        new LsCommandRepresentation(),
        new EchoCommandRepresentation(),
        new PwdCommandRepresentation(),
        new WcCommandRepresentation(),
        new GrepCommandRepresentation(),
        new ExitCommandRepresentation()
    },
    MetaCommands = new IMetaCommandRepresentation[]
    {
        new ContextSetCommandRepresentation(),
        new PipeCommandRepresentation()
    },
    ExternalCommandRepresentation = new ExternalCommandRepresentation()
};

var interpreter = new ConsoleInterpreter(tokenizer, commandsParser);

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Welcome to C# Bash Interpreter!\n");
Console.ResetColor();

var tokenSource = new CancellationTokenSource();
await interpreter.Execute(tokenSource.Token);