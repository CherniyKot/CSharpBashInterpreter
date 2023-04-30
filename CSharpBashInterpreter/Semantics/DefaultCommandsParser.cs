using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Exceptions;

namespace CSharpBashInterpreter.Semantics;

/// <summary>
/// Provide extendable parser with other predefined commands
/// </summary>
public class DefaultCommandsParser : ICommandParser
{
    /// <summary>
    /// Commands for additional processing input tokens
    /// </summary>
    public required IMetaCommandRepresentation[] MetaCommands { get; init; }

    /// <summary>
    /// Basic commands for executing
    /// </summary>
    public required ICommandRepresentation[] Commands { get; init; }

    /// <summary>
    /// Command for OS process calls
    /// </summary>
    public ICommandRepresentation? ExternalCommandRepresentation { get; init; }


    public ICommandExecutable Parse(string[] tokens, IContext context)
    {
        foreach (var metaCommand in MetaCommands.Where(x => x.CanBeParsed(tokens)))
            return metaCommand.Build(tokens, context, this);

        foreach (var command in Commands.Where(x => x.CanBeParsed(tokens)))
            return command.Build(tokens);

        if (ExternalCommandRepresentation is not null)
            return ExternalCommandRepresentation.Build(tokens);

        throw new ParseException(tokens);
    }
}