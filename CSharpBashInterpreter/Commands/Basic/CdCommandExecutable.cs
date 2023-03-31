using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

public class CdCommandExecutable : BaseCommandExecutable
{
    public CdCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    {
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        var args = Tokens.Skip(1).ToList();
        try
        {
            string path;
            if (args.Any())
            {
                path = args.First();
                var cur = ConsoleState.CurrentDirectory;
                path = path.StartsWith("/") ? path : $"{cur}/{path}";
            }
            else
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            ConsoleState.CurrentDirectory = path;
        }
        catch (Exception e)
        {
            await using var output = new StreamWriter(StreamSet.ErrorStream);
            await output.WriteLineAsync(e.Message);
            await output.FlushAsync();
            return 1;
        }
        return 0;
    }
}
