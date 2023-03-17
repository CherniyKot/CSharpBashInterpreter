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

    readonly PipeWrapper _leftPipe = new();
    readonly PipeWrapper _centralPipe = new();
    readonly PipeWrapper _rightPipe = new();

    public PipeCommandExecutable(IEnumerable<string> tokens,
        string delimiter, IContext context, ICommandParser parser,
        StreamSet streamSet) : base(tokens, streamSet)
    {
        _left = parser.Parse(Tokens.TakeWhile(x => x != delimiter), context,
            new StreamSet
            {
                InputStream = _leftPipe.ReaderStream,
                OutputStream = _centralPipe.WriterStream
            });
        _right = parser.Parse(Tokens.SkipWhile(x => x != delimiter).Skip(1), context,
            new StreamSet
            {
                InputStream = _centralPipe.ReaderStream,
                OutputStream = _rightPipe.WriterStream
            });
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var leftCopyTask = StreamSet.CopyToAsync(StreamSet.InputStream, _leftPipe.WriterStream);
        var rightCopyTask = StreamSet.CopyToAsync(_rightPipe.ReaderStream, StreamSet.OutputStream);
        
        var leftTask = _left.ExecuteAsync();
        var rightTask = _right.ExecuteAsync();
        
        Task.WaitAll(leftCopyTask, rightCopyTask);
        return (await Task.WhenAll(leftTask, rightTask)).FirstOrDefault(x => x != 0);
    }
}