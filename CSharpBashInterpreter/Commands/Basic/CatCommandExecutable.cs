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
    private const int BufferSize = 256;
    private readonly char[] _buffer = new char[BufferSize];

    public CatCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    public override async Task<int> Execute()
    {
        var args = Tokens.Skip(1).ToList();
        if (args.Any())
        {
            foreach (var fileName in args)
            {
                try
                {
                    if (fileName == "-")
                    {
                        while (InputStream.BaseStream.CanRead)
                        {
                            var bytesRead = await InputStream.ReadAsync(_buffer, 0, BufferSize);
                            await OutputStream.WriteAsync(_buffer, 0, bytesRead);
                            await OutputStream.FlushAsync();
                        }
                    }
                    else
                    {
                        using var fileStream = File.OpenText(fileName);
                        while (!fileStream.EndOfStream)
                        {
                            var bytesRead = await fileStream.ReadAsync(_buffer, 0, BufferSize);
                            await OutputStream.WriteAsync(_buffer, 0, bytesRead);
                            await OutputStream.FlushAsync();
                        }
                    }
                }
                catch (Exception e)
                {
                    await ErrorStream.WriteLineAsync(e.Message);
                    await ErrorStream.FlushAsync();
                    return 1;
                }
            }
        }
        else
        {
            try
            {
                while (InputStream.BaseStream.CanRead)
                {
                    var bytesRead = await InputStream.ReadAsync(_buffer, 0, BufferSize);
                    await OutputStream.WriteAsync(_buffer, 0, bytesRead);
                    await OutputStream.FlushAsync();
                }
            }
            catch (Exception e)
            {
                await ErrorStream.WriteLineAsync(e.Message);
                await ErrorStream.FlushAsync();
                return 1;
            }
        }
        return 0;
    }
}