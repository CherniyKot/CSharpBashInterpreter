namespace CSharpBashInterpreter.Commands.Basic;

public class PwdCommandExecutable : BaseCommandExecutable
{
    public PwdCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    public override async Task<int> Execute()
    {
        var args = Tokens.Skip(1);
        try
        {
            await ErrorStream.WriteLineAsync(Directory.GetCurrentDirectory());
            await ErrorStream.FlushAsync();
        }
        catch (Exception e)
        {
            await ErrorStream.WriteLineAsync(e.Message);
            await ErrorStream.FlushAsync();
            return 1;
        }
        await OutputStream.DisposeAsync();
        return 0;
    }
}
