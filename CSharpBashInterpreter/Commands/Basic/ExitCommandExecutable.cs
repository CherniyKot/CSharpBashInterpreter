using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Executable for exit command
///     exits the process with specified exit code (0 by default)
/// </summary>
public class ExitCommandExecutable : BaseCommandExecutable
{
    public ExitCommandExecutable(IEnumerable<string> tokens, StreamSet streamSet) : base(tokens, streamSet)
    {
    }

    protected override Task<int> ExecuteInternalAsync()
    {
        Environment.Exit(Tokens.Length > 1 ? int.Parse(Tokens[1]) : 0);
        return Task.FromResult(0);
    }
}