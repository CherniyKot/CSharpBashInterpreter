namespace CSharpBashInterpreter.Commands.Basic;

internal class EchoCommandExecutable : BaseCommandExecutable
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
                await OutputStream.WriteLineAsync(e.Message);
                await OutputStream.FlushAsync();
            }
        }

        await OutputStream.DisposeAsync();
        return 0;
    }
}
