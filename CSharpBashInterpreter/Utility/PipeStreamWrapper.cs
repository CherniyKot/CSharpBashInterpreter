namespace CSharpBashInterpreter.Utility;

public class PipeStreamWrapper : Stream
{
    public PipeStreamWrapper(PipeWrapper pipe, Stream stream)
    {
        InternalStream = stream;
        Pipe = pipe;
        pipe.CloseEvent += () => IsOpen = false;
    }

    private Stream InternalStream { get; }
    private PipeWrapper Pipe { get; }

    private bool IsOpen { get; set; } = true;

    public override bool CanRead => InternalStream.CanRead && (IsOpen || Length > Position);
    public override bool CanSeek => InternalStream.CanSeek && (IsOpen || Length > Position);
    public override bool CanWrite => InternalStream.CanWrite && (IsOpen || Length > Position);
    public override long Length => Pipe.Length;

    public override long Position
    {
        get => Pipe.Position;
        set => Pipe.Position = value;
    }

    public override void Flush()
    {
        InternalStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var result = InternalStream.Read(buffer, offset, count);
        Position += result;
        return result;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        Position = InternalStream.Seek(offset, origin);
        return Position;
    }

    public override void SetLength(long value)
    {
        Pipe.Length = value;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        Pipe.Length += count;
        InternalStream.Write(buffer, offset, count);
    }

    public override void Close()
    {
        base.Close();
        Pipe.Close();
    }
}