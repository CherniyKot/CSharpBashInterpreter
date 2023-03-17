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
    public CatCommandExecutable(IEnumerable<string> tokens, StreamSet streamSet) : base(tokens, streamSet)
    {
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var args = Tokens.Skip(1).ToList();
        if (args.Any())
            foreach (var fileName in args)
                try
                {
                    if (fileName == "-")
                    {
                        await StreamSet.InputStream.BaseStream.CopyToAsync(StreamSet.OutputStream.BaseStream);
                    }
                    else
                    {
                        await using var fileStream = File.OpenRead(fileName);
                        await fileStream.CopyToAsync(StreamSet.OutputStream.BaseStream);
                    }
                }
                catch (Exception e)
                {
                    await StreamSet.ErrorStream.WriteLineAsync(e.Message);
                    await StreamSet.ErrorStream.FlushAsync();
                    return 1;
                }
        else
            try
            {
                await StreamSet.InputStream.BaseStream.CopyToAsync(StreamSet.OutputStream.BaseStream);
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