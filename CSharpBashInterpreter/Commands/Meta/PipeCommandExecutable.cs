using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Meta;

public class PipeCommandExecutable : BaseCommandExecutable
{
    private readonly ICommandExecutable _left;
    private readonly ICommandExecutable _right;

    public PipeCommandExecutable(IEnumerable<string> tokens,
        string delimiter, IContext context, ICommandParser parser,
        StreamSet streamSet) : base(tokens, streamSet)
    {
        _left = parser.Parse(Tokens.TakeWhile(x => x != delimiter), context,
            new StreamSet
            {
                InputStream = new StreamReader(new MemoryStream()),
                OutputStream = new StreamWriter(new MemoryStream())
            });
        _right = parser.Parse(Tokens.SkipWhile(x => x != delimiter).Skip(1), context,
            new StreamSet
            {
                InputStream = new StreamReader(new MemoryStream()),
                OutputStream = new StreamWriter(new MemoryStream())
            });
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var leftCopyTask = StreamSet.OutputStream.BaseStream.CopyToAsync(_left.StreamSet.InputStream.BaseStream);
        var centralCopyTask =
            _left.StreamSet.OutputStream.BaseStream.CopyToAsync(_right.StreamSet.InputStream.BaseStream);
        var rightCopyTask = _right.StreamSet.OutputStream.BaseStream.CopyToAsync(StreamSet.OutputStream.BaseStream);
        await Task.WhenAll(leftCopyTask, rightCopyTask, centralCopyTask);
        return (await Task.WhenAll(_left.ExecuteAsync(), _right.ExecuteAsync())).FirstOrDefault(x => x != 0);
    }
}