using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Executable for bash cat command
///     Takes list of tokens starting with "cat" and processes the rest of them as arguments
///     Without arguments consumes strings from input stream snd copies them to the output stream
///     Arguments are interpreted as list of files and "-" strings
///     Concatenates contents of files (and input stream for "-") and copies result to the output stream
/// </summary>
public class CatCommandExecutable : BaseCommandExecutable
{
    public CatCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    {
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var args = Tokens.Skip(1).ToList();
        try
        {
            if (args.Any())
                foreach (var fileName in args)
                    if (fileName == "-")
                    {
                        await StreamSet.CopyToAsync(StreamSet.InputStream, StreamSet.OutputStream);
                    }
                    else
                    {
                        await using var fileStream = File.OpenRead(Path.Combine(ConsoleState.CurrentDirectory, fileName));
                        await StreamSet.CopyToAsync(fileStream, StreamSet.OutputStream);
                    }
            else
                await StreamSet.CopyToAsync(StreamSet.InputStream, StreamSet.OutputStream);
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