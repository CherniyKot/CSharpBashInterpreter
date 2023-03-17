using System.Buffers;
using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Meta;

public class PipeCommandExecutable : BaseCommandExecutable
{
    private readonly ICommandExecutable _left;
    private readonly ICommandExecutable _right;

    readonly PipeWrapper _centralPipe = new();

    public PipeCommandExecutable(IEnumerable<string> tokens,
        string delimiter, IContext context, ICommandParser parser,
        StreamSet streamSet) : base(tokens, streamSet)
    {
        _left = parser.Parse(Tokens.TakeWhile(x => x != delimiter), context,
            new StreamSet
            {
                InputStream = StreamSet.InputStream,
                OutputStream = _centralPipe.WriterStream
            });
        _right = parser.Parse(Tokens.SkipWhile(x => x != delimiter).Skip(1), context,
            new StreamSet
            {
                InputStream = _centralPipe.ReaderStream,
                OutputStream = StreamSet.OutputStream
            });
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var leftTask = await _left.ExecuteAsync();
        var rightTask = await _right.ExecuteAsync();

        return leftTask == 0 ? rightTask : leftTask;
    }
}