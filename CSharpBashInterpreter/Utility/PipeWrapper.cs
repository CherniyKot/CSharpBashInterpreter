using System.IO.Pipelines;

namespace CSharpBashInterpreter.Utility;

public class PipeWrapper
{
    public PipeStreamWrapper ReaderStream { get; }
    public PipeStreamWrapper WriterStream { get; }
    public Pipe Pipe { get; } = new Pipe();

    public event Action CloseEvent;

    public PipeWrapper()
    {
        ReaderStream = new PipeStreamWrapper(this, Pipe.Reader.AsStream());
        WriterStream = new PipeStreamWrapper(this, Pipe.Writer.AsStream());
    }

    public void Close()
    {
        CloseEvent();
        Pipe.Reader.Complete();
        Pipe.Writer.Complete();
    }
}