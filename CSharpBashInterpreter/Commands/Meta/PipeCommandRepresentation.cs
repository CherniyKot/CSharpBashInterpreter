using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Commands.Meta;

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

    public BaseCommandExecutable Build(IEnumerable<string> tokens, IContext context, ICommandParser parser)
    {
        return new PipeCommandExecutable(tokens, _delimiter, context, parser);
    }
}