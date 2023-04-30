using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics.Abstractions;

namespace CSharpBashInterpreter.Commands.Meta;

/// <summary>
/// Representation for bash pipes functionality
/// </summary>
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

    public ICommandExecutable Build(IEnumerable<string> input, IContext context, ICommandParser parser)
    {
        return new PipeCommandExecutable(input, _delimiter, context, parser);
    }
}