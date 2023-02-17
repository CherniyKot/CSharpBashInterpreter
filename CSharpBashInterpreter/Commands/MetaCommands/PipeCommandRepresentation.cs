using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Commands.MetaCommands;

public class PipeCommandRepresentation : IMetaCommandRepresentation
{
    private readonly string _delimiter = "|";

    public PipeCommandRepresentation(string? delimiter = null)
    {
        if (delimiter is not null)
            _delimiter = delimiter;
    }

    public bool CanBeParsed(IEnumerable<string> tokens)
    {
        return tokens.Contains(_delimiter);
    }

    public async Task<BaseCommandExecutable> Build(IEnumerable<string> tokens, ICommandParser parser)
    {
        var command = new PipeCommandExecutable(_delimiter, parser);
        await command.Initialize(tokens);
        return command;
    }
}