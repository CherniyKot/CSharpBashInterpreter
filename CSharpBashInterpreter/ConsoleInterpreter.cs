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

    public async Task Execute(CancellationToken token)
    {
        Console.CancelKeyPress += ConsoleCancelEventHandler;

        var context = new DefaultContext();

        while (!token.IsCancellationRequested)
            await ExecuteLoop(context);

        Console.CancelKeyPress -= ConsoleCancelEventHandler;
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
            var result = await command.ExecuteAsync();
            if (result != 0)
                PrintErrorToConsole($"Команда завершилась с кодом ошибки {result}.");
        }
        catch (Exception e)
        {
            PrintErrorToConsole(e.Message);
        }
    }

    private static void PrintErrorToConsole(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(message);
        Console.ResetColor();
    }

    private static void ConsoleCancelEventHandler(object? _, ConsoleCancelEventArgs eventArgs)
    {
        eventArgs.Cancel = true;
        InterruptableConsoleStream.Interrupt();
    }
}