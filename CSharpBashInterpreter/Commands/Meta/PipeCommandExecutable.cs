using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Commands.Meta;

public class PipeCommandExecutable : BaseCommandExecutable
{
    private readonly ICommandExecutable[] _commands;

    public PipeCommandExecutable(string input, string delimiter, IContext context, ICommandParser parser)
        : base(input.Split(delimiter, StringSplitOptions.TrimEntries))
    {
        _commands = Tokens.Select(x => parser.Parse(x, context)).ToArray();
    }

    public override async Task Execute()
    {
        _commands.First().InputStream = InputStream;
        _commands.Last().OutputStream = OutputStream;

        for (var i = 0; i < Tokens.Length - 1; i++)
        {
            var pipe = new Pipe();
            _commands[i].OutputStream = new StreamWriter(pipe.Writer.AsStream());
            _commands[i + 1].InputStream = new StreamReader(pipe.Reader.AsStream());
        }
        await Task.WhenAll(_commands.Select(x => x.Execute()));
    }
}