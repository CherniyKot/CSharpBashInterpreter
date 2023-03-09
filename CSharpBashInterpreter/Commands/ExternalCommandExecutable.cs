using System.Diagnostics;

namespace CSharpBashInterpreter.Commands;

/// <summary>
/// Executable for external commands (calls to OS processes)
/// </summary>
public class ExternalCommandExecutable : BaseCommandExecutable
{
    public ExternalCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var process = Process.Start(Tokens.First(),Tokens.Skip(1));
        await process.WaitForExitAsync();
        return process.ExitCode;
    }
}