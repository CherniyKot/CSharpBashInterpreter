using CSharpBashInterpreter.Commands;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Exceptions;

namespace CSharpBashInterpreter.Semantics;

public class DefaultCommandsParser : ICommandParser
{
    public required IMetaCommandRepresentation[] MetaCommands { get; init; }
    public required ICommandRepresentation[] Commands { get; init; }


    public ICommandExecutable Parse(string[] tokens, IContext context)
    {
        foreach (var metaCommand in MetaCommands.Where(x => x.CanBeParsed(tokens)))
            return metaCommand.Build(tokens, context, this);

        foreach (var command in Commands.Where(x => x.CanBeParsed(tokens)))
            return command.Build(tokens);

        var other = ProcessOtherCommand(tokens);
        if (other is not null)
            return other;

        throw new ParseException(tokens);
    }

    protected virtual BaseCommandExecutable? ProcessOtherCommand(IEnumerable<string> tokens) => null;
}