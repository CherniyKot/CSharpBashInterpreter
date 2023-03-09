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

    public override async Task<int> Execute()
    {
        var args = Tokens.Skip(1);
        try
        {
            var path = args.Any() ? args.First() : Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                await OutputStream.WriteAsync(Path.GetFileName(file) + '\n');
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