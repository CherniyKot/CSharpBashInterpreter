namespace CSharpBashInterpreter.Commands.Basic;


/// <summary>
/// Executable for bash echo command
/// Takes list of 2 tokens: "echo" and string
/// sends string to the output stream
/// </summary>
public class EchoCommandExecutable : BaseCommandExecutable
{
    public EchoCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    protected override async Task<int> ExecuteInternalAsync()
    {
        try
        {
            var concatArgs = string.Join(' ', Tokens.Skip(1));
            await OutputStream.WriteAsync(concatArgs);
            await OutputStream.FlushAsync();
        }
        catch (Exception e)
        {
            await ErrorStream.WriteLineAsync(e.Message);
            await ErrorStream.FlushAsync();
            return 1;
        }
        return 0;
    }
}
