namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Executable for bash ls command
/// Takes a list of tokens {"ls", directory} -> Consumes names of files in directory
/// Or takes a list of tokens {"ls"} -> Consumes names of files in current directory
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
            string path = args.Count() > 0 ? args.First() : Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                await OutputStream.WriteLineAsync(Path.GetFileName(file) + '\n');
                await OutputStream.FlushAsync();
            }
        }
        catch (Exception e)
        {
            await ErrorStream.WriteLineAsync(e.Message);
            await ErrorStream.FlushAsync();
            return 1;
        }

        await OutputStream.DisposeAsync();
        return 0;
    }
}