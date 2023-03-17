using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Executable for bash ls command
///     Takes a list of tokens starting with "ls"
///     Second token is path
///     If second token does not exist path is current path
///     Consumes names of files in path
/// </summary>
public class LsCommandExecutable : BaseCommandExecutable
{
    public LsCommandExecutable(IEnumerable<string> tokens, StreamSet streamSet) : base(tokens, streamSet)
    {
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var args = Tokens.Skip(1).ToList();
        try
        {
            var path = args.Any() ? args.First() : Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                await StreamSet.OutputStream.WriteLineAsync(Path.GetFileName(file));
                await StreamSet.OutputStream.FlushAsync();
            }
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