using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Executable for bash pwd command
/// Takes list of 1 token "pwd"
/// Returns current directory
/// </summary>
public class PwdCommandExecutable : BaseCommandExecutable
{
    public PwdCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    {
    }

    protected override async Task<int> ExecuteInternalAsync(StreamSet streamSet)
    {
        try
        {
            await using var outputStream = new StreamWriter(streamSet.OutputStream);
            await outputStream.WriteLineAsync(Directory.GetCurrentDirectory());
            await outputStream.FlushAsync();
        }
        catch (Exception e)
        {
            await using var errorStream = new StreamWriter(streamSet.ErrorStream);
            await errorStream.WriteLineAsync(e.Message);
            await errorStream.FlushAsync();
            return 1;
        }

        return 0;
    }
}