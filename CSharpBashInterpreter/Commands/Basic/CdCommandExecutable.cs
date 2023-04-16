using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Executable for bash cd command
///     Takes a list of tokens starting with "cd"
///     Second token is relative path or absolute path (starts with DirectorySeparatorChar)
///     If second token does not exist path is current path
///     Consumes names of files in path
/// </summary>
public class CdCommandExecutable : BaseCommandExecutable
{
    public CdCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    {
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var args = Tokens.Skip(1).ToList();
        try
        {
            if (args.Count > 1)
                throw new ArgumentException("Too many arguments");
            
            var path = args.Any() ? ConsoleState.ConvertPath(args.First())
                : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (!Directory.Exists(path))
                throw new IOException("Received path isn't a directory");
            
            ConsoleState.CurrentDirectory = path;
        }
        catch (Exception e)
        {
            await using var output = new StreamWriter(StreamSet.ErrorStream);
            await output.WriteLineAsync(e.Message);
            await output.FlushAsync();
            return 1;
        }
        return 0;
    }
}
