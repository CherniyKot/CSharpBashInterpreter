using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Executable for bash pwd command
///     Takes list of 1 token "pwd"
///     Returns current directory
/// </summary>
public class PwdCommandExecutable : BaseCommandExecutable
{
    public PwdCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    {
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        try
        {
            await using var outputStream = new StreamWriter(StreamSet.OutputStream);
            await outputStream.WriteLineAsync(ConsoleState.CurrentDirectory);
            await outputStream.FlushAsync();
        }
        catch (Exception e)
        {
            await using var errorStream = new StreamWriter(StreamSet.ErrorStream);
            await errorStream.WriteLineAsync(e.Message);
            await errorStream.FlushAsync();
            return 1;
        }

        return 0;
    }
}