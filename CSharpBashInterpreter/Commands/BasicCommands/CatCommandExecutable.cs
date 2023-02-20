namespace CSharpBashInterpreter.Commands.BasicCommands;

internal class CatCommandExecutable : BaseCommandExecutable
{
    private const int BufferSize = 256;
    private readonly char[] _buffer = new char[BufferSize];

    public override async Task Execute()
    {
        var args = Tokens.Skip(1);
        if (args.Any())
        {
            foreach (var fileName in args)
            {
                try
                {
                    using var fileStream = File.OpenText(fileName);
                    while (!fileStream.EndOfStream)
                    {
                        var bytesRead = await fileStream.ReadAsync(_buffer, 0, BufferSize);
                        await OutputStream.WriteAsync(_buffer, 0, bytesRead);
                        await OutputStream.FlushAsync();
                    }
                }
                catch (Exception e)
                {
                    await ErrorStream.WriteLineAsync(e.Message);
                    await ErrorStream.FlushAsync();
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
            }
        }

        await OutputStream.DisposeAsync();
    }
}