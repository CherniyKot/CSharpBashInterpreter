using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Meta;

/// <summary>
/// Executable for bash pipes functionality.
/// Use streams from <see cref="StreamSet"/>
/// and copy content from reading stream to writing stream
/// </summary>
public class PipeCommandExecutable : BaseCommandExecutable
{
    private readonly ICommandExecutable _left;
    private readonly ICommandExecutable _right;

    private readonly PipeWrapper _centralPipe = new();

    public PipeCommandExecutable(IEnumerable<string> tokens,
        string delimiter, IContext context, ICommandParser parser) : base(tokens)
    {
        _left = parser.Parse(Tokens.TakeWhile(x => x != delimiter), context);
        _right = parser.Parse(Tokens.SkipWhile(x => x != delimiter).Skip(1), context);
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var leftStreams = new StreamSet
        {
            InputStream = StreamSet.InputStream,
            OutputStream = _centralPipe.WriterStream
        };
        var rightStreams = new StreamSet
        {
            InputStream = _centralPipe.ReaderStream,
            OutputStream = StreamSet.OutputStream
        };

        var leftTask = await _left.ExecuteAsync(leftStreams);
        var rightTask = await _right.ExecuteAsync(rightStreams);

        return leftTask == 0 ? rightTask : leftTask;
    }
}