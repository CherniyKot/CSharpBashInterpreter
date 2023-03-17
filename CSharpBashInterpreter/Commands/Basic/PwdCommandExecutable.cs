using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Executable for bash pwd command
///     Takes list of 1 token "pwd"
///     Returns current directory
/// </summary>
public class PwdCommandExecutable : BaseCommandExecutable
{
    public PwdCommandExecutable(IEnumerable<string> tokens, StreamSet streamSet) : base(tokens, streamSet)
    {
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        try
        {
            await StreamSet.OutputStream.WriteLineAsync(Directory.GetCurrentDirectory());
            await StreamSet.OutputStream.FlushAsync();
        }
        catch (Exception e)
        {
            await StreamSet.ErrorStream.WriteLineAsync(e.Message);
            await StreamSet.ErrorStream.FlushAsync();
            return 1;
        }

        return 0;
    }
}