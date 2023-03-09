using System.IO;
using System.IO.Pipes;
using System.Text;

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
    private const int BufferSize = 256;
    private readonly char[] _buffer = new char[BufferSize];

    public WcCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    public override async Task<int> Execute()
    {
        var args = Tokens.Skip(1).ToList();
        if (args.Any())
        {
            long totalLines = 0;
            long totalWords = 0;
            long totalBytes = 0;
            foreach (var fileName in args)
            {
                long lines = 0;
                long words = 0;
                long bytes = new FileInfo(fileName).Length;
                try
                {
                    using var fileStream = File.OpenText(fileName);
                    
                    while (!fileStream.EndOfStream)
                    {
                        var s = await fileStream.ReadLineAsync();
                        if (string.IsNullOrEmpty(s)) continue;

                        lines++;
                        words += s.Split().Length;
                    }

                    totalLines += lines;
                    totalBytes += bytes;
                    totalWords += words;
                    await OutputStream.WriteLineAsync($"{lines} {words} {bytes} {fileName}");
                }
                catch (Exception e)
                {
                    await ErrorStream.WriteLineAsync(e.Message);
                    await ErrorStream.FlushAsync();
                    return 1;
                }
            }

            if (args.Count > 1)
            {
                await OutputStream.WriteLineAsync($"{totalLines} {totalWords} {totalBytes} Total");
            }

        }
        else
        {
            try
            {
                int lines = 0;
                int words = 0;
                int bytes = 0;
                var encoding = InputStream.CurrentEncoding;
                while (InputStream.BaseStream.CanRead)
                {
                    var s = await InputStream.ReadLineAsync();
                    if (string.IsNullOrEmpty(s)) continue;

                    lines++;
                    words += s.Split().Length;
                    bytes += encoding.GetByteCount(s+ Environment.NewLine);
                }
                await OutputStream.WriteLineAsync($"{lines} {words} {bytes}");
            }
            catch (Exception e)
            {
                await ErrorStream.WriteLineAsync(e.Message);
                await ErrorStream.FlushAsync();
                return 1;
            }
        }

        await OutputStream.DisposeAsync();
        return 0;
    }
}