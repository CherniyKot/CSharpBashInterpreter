using System.IO.Pipelines;
using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Commands.Meta;

public class PipeCommandExecutable : BaseCommandExecutable
{
    private readonly BaseCommandExecutable _left;
    private readonly BaseCommandExecutable _right;
    private readonly Pipe _pipe;

    public PipeCommandExecutable(IEnumerable<string> tokens, string delimiter, IContext context, ICommandParser parser)
        : base(tokens)
    {
        _left = parser.Parse(Tokens.TakeWhile(t => t != delimiter), context);
        _right = parser.Parse(Tokens.SkipWhile(t => t != delimiter).Skip(1), context);

        _pipe = new Pipe();
        _left.OutputStream = new StreamWriter(_pipe.Writer.AsStream());
        _right.InputStream = new StreamReader(_pipe.Reader.AsStream());
    }

    public override async Task Execute()
    {
        var leftTask = _left.Execute();
        var rightTask = _right.Execute();

        await Task.WhenAll(leftTask, rightTask);
    }
}