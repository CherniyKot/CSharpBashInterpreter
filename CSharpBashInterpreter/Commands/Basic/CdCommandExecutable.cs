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
            if (!args.Any())
            {
                throw new Exception("cd takes 1 argument --- path to the directory");
            }
            var path = args.First();
            var cur = Directory.GetCurrentDirectory();
            if (path.StartsWith("/"))
            {
                Directory.SetCurrentDirectory(path);
            }
            else
            {
                Directory.SetCurrentDirectory($"{cur}/{path}");
            }
        }
        catch (Exception e)
        {
            await using var output = new StreamWriter(StreamSet.ErrorStream);
            await output.WriteLineAsync(e.Message);
            await output.FlushAsync();
            return 1;
        }
        return 0;
        throw new NotImplementedException();
    }
}
