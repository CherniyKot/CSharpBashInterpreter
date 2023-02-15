using CSharpBashInterpreter.Commands.BasicCommands;
using CSharpBashInterpreter.Commands.Factory;
using CSharpBashInterpreter.Commands.MetaCommands;
using CSharpBashInterpreter.Commands.MetaCommands.Utility;
using CSharpBashInterpreter.Semantics;

Parser.MetaCommands.AddRange(new[] { new CommandFactory<Pipe>() });
Parser.Commands.AddRange(new []{new CommandFactory<CatCommand>()});

Console.CancelKeyPress += (sender, eventArgs) =>
{
    eventArgs.Cancel = true;
    InterruptableConsoleStream.Interrupt();
};

while (true)
{
    try
    {
        var line = Console.ReadLine();
        var tokens = Tokenizer.Tokenize(line ?? "");
        var command = Parser.Parse(tokens);
        await command.Run();
    }
    catch (Exception e)
    {
        Console.Error.WriteLine(e.Message);
    }
}