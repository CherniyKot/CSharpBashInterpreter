using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Executable for bash wc command
/// Takes list of tokens starting with "wc" and processes the rest of them as arguments
/// Without arguments processes strings from input stream as input
/// Arguments are interpreted as list of files
/// Counts number of lines, words, bytes in input and writes it to the output stream
/// </summary>
public class WcCommandExecutable : BaseCommandExecutable
{
    public WcCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    {
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var args = Tokens.Skip(1).ToList();
        await using var outputStream = new StreamWriter(StreamSet.OutputStream);
        if (args.Any())
        {
            long totalLines = 0;
            long totalWords = 0;
            long totalBytes = 0;
            foreach (var fileName in args)
            {
                long lines = 0;
                long words = 0;
                var bytes = new FileInfo(fileName).Length;
                try
                {
                    using var fileStream = File.OpenText(fileName);

                    while (!fileStream.EndOfStream)
                    {
                        var s = await fileStream.ReadLineAsync()??"";
                        lines++;
                        words += s.Split().Length;
                    }

                    totalLines += lines;
                    totalBytes += bytes;
                    totalWords += words;
                    await outputStream.WriteLineAsync($"{lines} {words} {bytes} {fileName}");
                }
                catch (Exception e)
                {
                    await using var errorStream = new StreamWriter(StreamSet.ErrorStream);
                    await errorStream.WriteLineAsync(e.Message);
                    await errorStream.FlushAsync();
                    return 1;
                }
            }

            if (args.Count > 1)
                await outputStream.WriteLineAsync($"{totalLines} {totalWords} {totalBytes} Total");
        }
        else
        {
            using var inputStream = new StreamReader(StreamSet.InputStream);
            try
            {
                var lines = 0;
                var words = 0;
                var bytes = 0;
                var encoding = inputStream.CurrentEncoding;
                while (StreamSet.InputStream.CanRead)
                {
                    var result = await inputStream.ReadLineAsync() ?? "";
                    lines++;
                    words += result.Split().Length;
                    bytes += encoding.GetByteCount(result + Environment.NewLine);
                }

                await outputStream.WriteLineAsync($"{lines} {words} {bytes}");
            }
            catch (Exception e)
            {
                await using var errorStream = new StreamWriter(StreamSet.ErrorStream);
                await errorStream.WriteLineAsync(e.Message);
                await errorStream.FlushAsync();
                return 1;
            }
        }
        return 0;
    }
}