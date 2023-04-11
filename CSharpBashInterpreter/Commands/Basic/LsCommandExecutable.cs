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
            var path = $"{ConsoleState.CurrentDirectory}/{args.FirstOrDefault()}";
            var attributes = File.GetAttributes(path);
            
            await using var outputStream = new StreamWriter(StreamSet.OutputStream);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                var dirs = Directory.GetDirectories(path);
                foreach (var dir in dirs)
                    await outputStream.WriteLineAsync($"{Path.GetFileName(dir)}{Path.DirectorySeparatorChar}");
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                    await outputStream.WriteLineAsync(Path.GetFileName(file));
            }
            else
            {
                await outputStream.WriteLineAsync(args.First());
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
