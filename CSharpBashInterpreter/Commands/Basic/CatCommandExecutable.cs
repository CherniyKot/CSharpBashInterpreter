using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Executable for bash cat command
/// Takes list of tokens starting with "cat" and processes the rest of them as arguments
/// Without arguments consumes strings from input stream snd copies them to the output stream
/// Arguments are interpreted as list of files and "-" strings
/// Concatenates contents of files (and input stream for "-") and copies result to the output stream
/// </summary>
public class CatCommandExecutable : BaseCommandExecutable
{
    public CatCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    {
    }

    protected override async Task<int> ExecuteInternalAsync(StreamSet streamSet)
    {
        var args = Tokens.Skip(1).ToList();
        try
        {
            if (args.Any())
                foreach (var fileName in args)
                    if (fileName == "-")
                    {
                        await StreamSet.CopyToAsync(streamSet.InputStream, streamSet.OutputStream);
                    }
                    else
                    {
                        await using var fileStream = File.OpenRead(fileName);
                        await StreamSet.CopyToAsync(fileStream, streamSet.OutputStream);
                    }
            else
                await StreamSet.CopyToAsync(streamSet.InputStream, streamSet.OutputStream);
        }
        catch (Exception e) when(e is InvalidOperationException or NotSupportedException or IOException)
        {
            await using var errorStream = new StreamWriter(streamSet.ErrorStream);
            await errorStream.WriteLineAsync(e.Message);
            await errorStream.FlushAsync();
            return 1;
        }

        await using var writeStream = new StreamWriter(streamSet.OutputStream);
        await writeStream.WriteLineAsync();
        await writeStream.FlushAsync();
        return 0;
    }
}