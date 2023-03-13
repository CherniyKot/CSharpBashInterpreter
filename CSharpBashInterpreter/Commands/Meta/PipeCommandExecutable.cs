using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics;
using CSharpBashInterpreter.Semantics.Abstractions;

namespace CSharpBashInterpreter.Commands.Meta;

public class PipeCommandExecutable : BaseCommandExecutable
{
    private readonly ICommandExecutable _left;
    private readonly ICommandExecutable _right;

    public PipeCommandExecutable(IEnumerable<string> tokens, string delimiter, IContext context, ICommandParser parser)  : base(tokens)
    {
        var pipe = new Pipe();
        _left = parser.Parse(Tokens.TakeWhile(x => x != delimiter), context);
        _right = parser.Parse(Tokens.SkipWhile(x => x != delimiter).Skip(1), context);
        _left.InputStream = InputStream;
        _left.OutputStream = new StreamWriter(pipe.Writer.AsStream());
        _right.InputStream = new StreamReader(pipe.Reader.AsStream());
        _right.OutputStream = OutputStream;
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        return (await Task.WhenAll(_left.ExecuteAsync(), _right.ExecuteAsync())).FirstOrDefault(x => x != 0);
    }
}