﻿using CSharpBashInterpreter.Semantics;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter;

public sealed class ConsoleInterpreter
{
    private readonly ITokenizer _tokenizer;
    private readonly ICommandParser _commandParser;
    private readonly IContextManager _contextManager;

    public ConsoleInterpreter(ITokenizer tokenizer, ICommandParser commandParser, IContextManager contextManager)
    {
        _tokenizer = tokenizer;
        _commandParser = commandParser;
        _contextManager = contextManager;

        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    public async Task Execute(CancellationToken token)
    {
        Console.CancelKeyPress += ConsoleCancelEventHandler;

        var context = _contextManager.GenerateContext();

        while (!token.IsCancellationRequested)
            await ExecuteLoop(context);

        Console.CancelKeyPress -= ConsoleCancelEventHandler;
    }

    private async Task ExecuteLoop(IContext context)
    {
        try
        {
            var line = Console.ReadLine() ?? "";
            var substituteLine = _contextManager.SubstituteVariablesInText(line, context);
            var tokens = _tokenizer.Tokenize(substituteLine);
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