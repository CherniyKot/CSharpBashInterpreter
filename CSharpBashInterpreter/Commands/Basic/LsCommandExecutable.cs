namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Executable for bash ls command
/// Takes a list of tokens starting with "ls"
/// Second token is path
/// If second token does not exist path is current path
/// Consumes names of files in path
/// </summary>
public class LsCommandExecutable : BaseCommandExecutable
{
    public LsCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var args = Tokens.Skip(1).ToList();
        try
        {
            var path = args.Any() ? args.First() : Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                await OutputStream.WriteLineAsync(Path.GetFileName(file));
                await OutputStream.FlushAsync();
            }
        }
        catch (Exception e)
        {
            await ErrorStream.WriteLineAsync(e.Message);
            await ErrorStream.FlushAsync();
            return 1;
        }
        return 0;
    }
}