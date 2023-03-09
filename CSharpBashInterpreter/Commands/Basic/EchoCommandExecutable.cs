namespace CSharpBashInterpreter.Commands.Basic;


/// <summary>
/// Executable for bash echo command
/// Takes list of 2 tokens: "echo" and string
/// sends string to the output stream
/// </summary>

public class EchoCommandExecutable : BaseCommandExecutable
{
    private const int BufferSize = 256;
    private readonly char[] _buffer = new char[BufferSize];

    public EchoCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    public override async Task<int> Execute()
    {
        var args = Tokens.Skip(1);
        if (args.Any())
    {
        try
        {
                await OutputStream.WriteLineAsync(args.First());
                await OutputStream.FlushAsync();
            }
        catch (Exception e)
        {
            await ErrorStream.WriteLineAsync(e.Message);
            await ErrorStream.FlushAsync();
            return 1;
        }
        }
        return 0;
    }
}
