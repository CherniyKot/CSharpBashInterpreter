using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Meta;

public class PipeCommandRepresentation : IMetaCommandRepresentation
{
    private readonly string _delimiter = "|";

    public PipeCommandRepresentation(string? delimiter = null)
    {
        if (delimiter is not null)
            _delimiter = delimiter;
    }

    public bool CanBeParsed(IEnumerable<string> data)
    {
        return data.Contains(_delimiter);
    }

    public ICommandExecutable Build(IEnumerable<string> input, IContext context, ICommandParser parser,
        StreamSet streamSet)
    {
        return new PipeCommandExecutable(input, _delimiter, context, parser, streamSet);
    }
}