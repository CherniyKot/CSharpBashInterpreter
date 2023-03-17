using System.IO.Pipelines;

namespace CSharpBashInterpreter.Utility;

public class PipeStreamWrapper : Stream
{
    private Stream InternalStream { get; }
    private PipeWrapper Pipe { get; }
    
    private bool IsOpen { get; set; } = true;

    public PipeStreamWrapper(PipeWrapper pipe, Stream stream)
    {
        InternalStream = stream;
        Pipe = pipe;
        pipe.CloseEvent += () => IsOpen = false;
    }

    public override void Flush()
    {
        InternalStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return InternalStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return InternalStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        InternalStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        InternalStream.Write(buffer, offset, count);
    }

    public override void Close()
    {
        base.Close();
        Pipe.Close();
    }

    public override bool CanRead => InternalStream.CanRead && IsOpen;
    public override bool CanSeek => InternalStream.CanSeek && IsOpen;
    public override bool CanWrite => InternalStream.CanWrite && IsOpen;
    public override long Length => InternalStream.Length;

    public override long Position
    {
        get => InternalStream.Position;
        set => InternalStream.Position = value;
    }
}