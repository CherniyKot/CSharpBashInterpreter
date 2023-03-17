namespace CSharpBashInterpreter.Utility;

public class StreamSet : IAsyncDisposable
{
    public StreamReader InputStream { get; set; } = new(new InterruptableConsoleStream());
    public StreamWriter OutputStream { get; set; } = new(Console.OpenStandardOutput());
    public StreamWriter ErrorStream { get; set; } = new(Console.OpenStandardError());

    public async ValueTask DisposeAsync()
    {
        InputStream.Dispose();
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
}