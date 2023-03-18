using CSharpBashInterpreter.Commands.Abstractions;

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
    public LsCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    {
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var args = Tokens.Skip(1).ToList();
        try
        {
            var path = args.Any() ? args.First() : Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(path);
            await using var outputStream = new StreamWriter(StreamSet.OutputStream);
            foreach (var file in files)
            {
                await outputStream.WriteLineAsync(Path.GetFileName(file));
            }
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