using CSharpBashInterpreter.Commands;
using CSharpBashInterpreter.Commands.BasicCommands;
using CSharpBashInterpreter.Commands.MetaCommands;
using CSharpBashInterpreter.Exceptions;

namespace CSharpBashInterpreter.Semantics;

public class DefaultCommandsParser : ICommandParser
{
    public required IMetaCommandRepresentation[] MetaCommands { get; init; }
    public required ITerminalCommandRepresentation[] Commands { get; init; }
    
    public virtual async Task<BaseCommandExecutable> Parse(IEnumerable<string> tokens)
    {
        foreach (var metaCommand in MetaCommands.Where(x => x.CanBeParsed(tokens)))
            return await metaCommand.Build(tokens, this);

        foreach (var metaCommand in Commands.Where(x => x.CanBeParsed(tokens)))
            return await metaCommand.Build(tokens);

        var other = await ProcessOtherCommand(tokens);
        if (other is not null)
            return other;

        throw new ParseException(tokens);
    }

    protected virtual Task<BaseCommandExecutable?> ProcessOtherCommand(IEnumerable<string> tokens) =>
        Task.FromResult<BaseCommandExecutable?>(null);
}