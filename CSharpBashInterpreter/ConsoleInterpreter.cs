using System.Text;
using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Semantics.Context;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter;

public sealed class ConsoleInterpreter
{
    private readonly ICommandParser _commandParser;
    private readonly ITokenizer _tokenizer;

    public ConsoleInterpreter(ITokenizer tokenizer, ICommandParser commandParser)
    {
        _tokenizer = tokenizer;
        _commandParser = commandParser;

        Console.OutputEncoding = Encoding.UTF8;
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
        Console.Write($"[{DateTime.Now.ToShortTimeString()}] SharpBash> ");
        try
        {
            var line = Console.ReadLine() ?? "";
            var substituteLine = context.SubstituteVariablesInText(line);
            var tokens = _tokenizer.Tokenize(substituteLine);
            if (tokens.Length == 0)
                return;
            var command = _commandParser.Parse(tokens, context);
            await using var ioStreams = new StreamSet();
            var result = await command.ExecuteAsync(ioStreams);
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