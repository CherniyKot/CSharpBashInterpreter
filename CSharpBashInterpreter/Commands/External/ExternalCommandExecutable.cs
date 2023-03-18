using System.Diagnostics;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics.Abstractions;

namespace CSharpBashInterpreter.Commands.External;

/// <summary>
///     Executable for external commands (calls to OS processes)
/// </summary>
public class ExternalCommandExecutable : BaseCommandExecutable
{
    private readonly IContext _context;

    public ExternalCommandExecutable(IEnumerable<string> tokens, IContext context) : base(tokens)
    {
        _context = context;
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var startInfo = new ProcessStartInfo(Tokens.First());

        foreach (var argument in Tokens.Skip(1))
            startInfo.ArgumentList.Add(argument);

        foreach (var (key, value) in _context.EnvironmentVariables)
            startInfo.EnvironmentVariables.Add(key, value);

        Process? process = null;
        try
        {
            process = Process.Start(startInfo)!;
            await process.WaitForExitAsync();
        }
        catch (Exception e)
        {
            await using var errorStream = new StreamWriter(StreamSet.ErrorStream);
            await errorStream.WriteLineAsync(e.Message);
            await errorStream.FlushAsync();
        }

        return process?.ExitCode ?? 1;
    }
}