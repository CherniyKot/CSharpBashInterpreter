namespace CSharpBashInterpreter.Commands.Meta.Utility;

public class InterruptableConsoleStream : Stream
{
    private readonly Stream _baseStream = Console.OpenStandardInput();
    private static bool _isInterrupted = false;

    public InterruptableConsoleStream()
    {
        _isInterrupted = false;
    }
    public static void Interrupt()
    {
        _isInterrupted = true;
    }

    public override void Flush()
    {
        _baseStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _baseStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _baseStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _baseStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _baseStream.Write(buffer, offset, count);
    }

    public override bool CanRead => _baseStream.CanRead && !_isInterrupted;
    public override bool CanSeek => _baseStream.CanSeek && !_isInterrupted;
    public override bool CanWrite => _baseStream.CanWrite && !_isInterrupted;
    public override long Length => _baseStream.Length;

    public override long Position
    {
        get => _baseStream.Position;
        set => _baseStream.Position = value;
    }
}