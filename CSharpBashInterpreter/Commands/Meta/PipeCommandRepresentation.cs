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

    public bool CanBeParsed(string data)
    {
        return data.Contains(_delimiter);
    }

    public IEnumerable<string> Process(string input, IContext context, ICommandParser parser)
    {
        throw new NotImplementedException();
    }

    public ICommandExecutable Build(string input, IContext context, ICommandParser parser)
    {
        return new PipeCommandExecutable(input, _delimiter, context, parser);
    }
}