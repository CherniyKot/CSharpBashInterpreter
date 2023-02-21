using CSharpBashInterpreter.Commands;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Exceptions;

namespace CSharpBashInterpreter.Semantics;

public sealed class DefaultCommandsParser : ICommandParser
{
    public required IMetaCommandRepresentation[] MetaCommands { get; init; }
    public required ICommandRepresentation[] Commands { get; init; }

    private readonly ITokenizer _tokenizer;

    public DefaultCommandsParser(ITokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public ICommandExecutable Parse(string input, IContext context)
    {
        foreach (var metaCommand in MetaCommands.Where(x => x.CanBeParsed(input)))
            return metaCommand.Build(input, context, this);

        var tokens = _tokenizer.Tokenize(input);

        foreach (var command in Commands.Where(x => x.CanBeParsed(tokens)))
            return command.Build(tokens);

        var other = ProcessOtherCommand(tokens);
        if (other is not null)
            return other;

        throw new ParseException(tokens);
    }

    private BaseCommandExecutable? ProcessOtherCommand(IEnumerable<string> tokens) => null;
}