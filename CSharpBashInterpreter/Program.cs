using CSharpBashInterpreter;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Commands.External;
using CSharpBashInterpreter.Commands.Meta;
using CSharpBashInterpreter.Semantics;


var tokenizer = new SpaceTokenizer();
var contextManager = new DefaultContextManager();
var commandsParser = new DefaultCommandsParser
{
    Commands = new ICommandRepresentation[]
    {
        new CatCommandRepresentation(),
        new LsCommandRepresentation(),
        new EchoCommandRepresentation(),
        new PwdCommandRepresentation(),
        new WcCommandRepresentation(),
        new ExitCommandRepresentation()
    },
    MetaCommands = new IMetaCommandRepresentation[]
    {
        new ContextSetCommandRepresentation()
    },
    ExternalCommandRepresentation = new ExternalCommandRepresentation()
};

var interpreter = new ConsoleInterpreter(tokenizer, commandsParser, contextManager);

var tokenSource = new CancellationTokenSource();
await interpreter.Execute(tokenSource.Token);

