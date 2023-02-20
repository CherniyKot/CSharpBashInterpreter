namespace CSharpBashInterpreter.Commands.Basic;

public class LsCommandExecutable : BaseCommandExecutable
{
    public LsCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    public override async Task Execute()
    {
        var files = Directory.GetFiles(Directory.GetCurrentDirectory());
        foreach (var file in files)
        {
            await OutputStream.WriteAsync(Path.GetFileName(file) + '\n');
            await OutputStream.FlushAsync();
        }
    }
}