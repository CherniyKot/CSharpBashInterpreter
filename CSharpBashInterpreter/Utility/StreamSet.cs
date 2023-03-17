using System.Buffers;

namespace CSharpBashInterpreter.Utility;

public class StreamSet : IAsyncDisposable
{
    public Stream InputStream { get; set; } = new InterruptableConsoleStream();
    public Stream OutputStream { get; set; } = Console.OpenStandardOutput();
    public Stream ErrorStream { get; set; } = Console.OpenStandardError();

    public async ValueTask DisposeAsync()
    {
        await InputStream.DisposeAsync();
        await OutputStream.DisposeAsync();
        await ErrorStream.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public void Close()
    {
        OutputStream.Close();
        InputStream.Close();
        ErrorStream.Close();
    }


    public static async Task CopyToAsync(Stream source, Stream destination)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(256);
        try
        {
            while (true)
            {
                int bytesRead = await source.ReadAsync(new Memory<byte>(buffer)).ConfigureAwait(false);
                await destination.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, bytesRead)).ConfigureAwait(false);
            }
        }
        catch (Exception e) when (e is InvalidOperationException or NotSupportedException)
        {
            source.Close();
            destination.Close();
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}