using System.Diagnostics;

namespace CSharpBashInterpreter.Commands;

public class ExternalCommandExecutable : BaseCommandExecutable
{
    public ExternalCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    public override async Task<int> Execute()
    {
        var process = Process.Start(Tokens.First(),Tokens.Skip(1));
        await process.WaitForExitAsync();
        return process.ExitCode;
    }
}