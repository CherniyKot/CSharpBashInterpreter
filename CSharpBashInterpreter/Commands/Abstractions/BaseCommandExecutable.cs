using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
/// Base class with boilerplate for command executables
/// </summary>
public abstract class BaseCommandExecutable : ICommandExecutable
{
    protected readonly string[] Tokens;

    protected BaseCommandExecutable(IEnumerable<string> tokens) =>
        Tokens = tokens.ToArray();

    public async Task<int> ExecuteAsync(StreamSet streams)
    {
        var result = await ExecuteInternalAsync(streams);
        streams.Close();
        return result;
    }

    protected abstract Task<int> ExecuteInternalAsync(StreamSet streamSet);
}