using System.IO.Pipelines;

namespace CSharpBashInterpreter.Utility;

public class PipeWrapper
{
    public PipeStreamWrapper ReaderStream { get; }
    public PipeStreamWrapper WriterStream { get; }
    private Pipe Pipe { get; } = new();

    public long Position { get; set; }
    public long Length { get; set; }

    public event Action CloseEvent;

    public PipeWrapper()
    {
        ReaderStream = new PipeStreamWrapper(this, Pipe.Reader.AsStream());
        WriterStream = new PipeStreamWrapper(this, Pipe.Writer.AsStream());
    }

    public void Close()
    {
        CloseEvent();
        // Pipe.Reader.Complete();
        // Pipe.Writer.Complete();
    }
}