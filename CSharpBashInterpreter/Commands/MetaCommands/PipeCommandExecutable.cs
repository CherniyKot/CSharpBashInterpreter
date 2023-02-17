using System.IO.Pipelines;
using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Commands.MetaCommands;

public class PipeCommandExecutable : BaseCommandExecutable
{
    private readonly ICommandParser _parser;
    private readonly string _delimiter;
        
    private BaseCommandExecutable? _left;
    private BaseCommandExecutable? _right;

    private Pipe? _pipe;

    public PipeCommandExecutable(string delimiter, ICommandParser parser)
    {
        _delimiter = delimiter;
        _parser = parser;
    }
        
    public override async Task Initialize(IEnumerable<string> tokens)
    {
        var tokenList = tokens.ToList();
        _left = await _parser.Parse(tokenList.TakeWhile(t => t != _delimiter));
        _right = await _parser.Parse(tokenList.SkipWhile(t => t != _delimiter).Skip(1));

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