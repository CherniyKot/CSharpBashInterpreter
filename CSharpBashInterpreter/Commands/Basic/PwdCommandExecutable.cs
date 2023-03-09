namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Executable for bash pwd command
/// Takes list of 1 token "pwd"
/// Returns current directory
/// </summary>
/// 
public class PwdCommandExecutable : BaseCommandExecutable
{
    public PwdCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    public override async Task<int> Execute()
    {
        try
        {
            await OutputStream.WriteLineAsync(Directory.GetCurrentDirectory());
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
