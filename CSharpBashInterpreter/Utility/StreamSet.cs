using System.Buffers;

namespace CSharpBashInterpreter.Utility;

public class StreamSet : IAsyncDisposable
{
    public Stream InputStream { get; init; } = new InterruptableConsoleStream();
    public Stream OutputStream { get; init; } = Console.OpenStandardOutput();
    public Stream ErrorStream { get; init; } = Console.OpenStandardError();

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
        bool canTakePositionAndLength = false;
        try
        {
            long a = source.Length;
            a = source.Position;
            canTakePositionAndLength = true;
        }
        catch
        {
        }
        try
        {
            while (true)
            {
                var bytesRead = await source.ReadAsync(new Memory<byte>(buffer));
                await destination.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, bytesRead));
                await destination.FlushAsync();
                if (canTakePositionAndLength && source.Length == source.Position)
                {
                    break;
                }
            }
        }
        catch (Exception e) when (e is InvalidOperationException or NotSupportedException)
        {
            throw;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}