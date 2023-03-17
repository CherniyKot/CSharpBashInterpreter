using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Exceptions;
using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Semantics.Parsing;

/// <summary>
///     Provide extendable parser with other predefined commands
/// </summary>
public class DefaultCommandsParser : ICommandParser
{
    /// <summary>
    ///     Commands for additional processing input tokens
    /// </summary>
    public required IMetaCommandRepresentation[] MetaCommands { get; init; }

    /// <summary>
    ///     Basic commands for executing
    /// </summary>
    public required ICommandRepresentation[] Commands { get; init; }

    /// <summary>
    ///     Command for OS process calls
    /// </summary>
    public IExternalCommandRepresentation? ExternalCommandRepresentation { get; init; }


    public ICommandExecutable Parse(IEnumerable<string> tokens, IContext context, StreamSet streamSet)
    {
        foreach (var metaCommand in MetaCommands.Where(x => x.CanBeParsed(tokens)))
            return metaCommand.Build(tokens, context, this, streamSet);

        foreach (var command in Commands.Where(x => x.CanBeParsed(tokens)))
            return command.Build(tokens, streamSet);

        if (ExternalCommandRepresentation is not null)
            return ExternalCommandRepresentation.Build(tokens, context, streamSet);

        throw new ParseException(tokens);
    }
}