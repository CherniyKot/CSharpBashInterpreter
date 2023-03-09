using CSharpBashInterpreter.Commands.Meta.Utility;
using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter;

public sealed class ConsoleInterpreter
{
    private readonly ITokenizer _tokenizer;
    private readonly ICommandParser _commandParser;

    public ConsoleInterpreter(ITokenizer tokenizer, ICommandParser commandParser)
    {
        _tokenizer = tokenizer;
        _commandParser = commandParser;

        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    public async Task Execute(CancellationTokenSource source)
    {
        ConsoleCancelEventHandler handler = (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            source.Cancel();
            InterruptableConsoleStream.Interrupt();
        };
        Console.CancelKeyPress += handler;

        var token = source.Token;
        var context = new DefaultContext();

        while (!token.IsCancellationRequested)
            await ExecuteLoop(context);

        Console.CancelKeyPress -= handler;
    }

    private async Task ExecuteLoop(IContext context)
    {
        try
        {
            var line = Console.ReadLine() ?? "";
            var tokens = _tokenizer.Tokenize(line);
            if (tokens.Length == 0)
                return;
            await using var command = _commandParser.Parse(tokens, context);
            var result = await command.Execute();
            if (result != 0)
                Console.WriteLine($"Команда завершилась с кодом ошибки {result}.");
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
        }
    }
}