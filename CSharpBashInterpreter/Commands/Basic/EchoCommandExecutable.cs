using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Executable for bash echo command
///     Takes list of 2 tokens: "echo" and string
///     sends string to the output stream
/// </summary>
public class EchoCommandExecutable : BaseCommandExecutable
{
    public EchoCommandExecutable(IEnumerable<string> tokens, StreamSet streamSet) : base(tokens, streamSet)
    {
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        try
        {
            var concatArgs = string.Join(' ', Tokens.Skip(1));
            await StreamSet.OutputStream.WriteLineAsync(concatArgs);
            await StreamSet.OutputStream.FlushAsync();
        }
        catch (Exception e)
        {
            await StreamSet.ErrorStream.WriteLineAsync(e.Message);
            await StreamSet.ErrorStream.FlushAsync();
            return 1;
        }

        return 0;
    }
}