using CSharpBashInterpreter;
using CSharpBashInterpreter.Commands;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Semantics;

var tokenizer = new SpaceTokenizer();
var commandsParser = new DefaultCommandsParser()
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
    MetaCommands = new IMetaCommandRepresentation[] { },
    ExternalCommandRepresentation = new ExternalCommandRepresentation()
};

var interpreter = new ConsoleInterpreter(tokenizer, commandsParser);

var tokenSource = new CancellationTokenSource();
await interpreter.Execute(tokenSource.Token);

