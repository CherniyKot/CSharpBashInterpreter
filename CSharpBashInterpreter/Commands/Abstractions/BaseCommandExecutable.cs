using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
///     Base class with boilerplate for command executables
/// </summary>
public abstract class BaseCommandExecutable : ICommandExecutable
{
    protected readonly string[] Tokens;

    protected BaseCommandExecutable(IEnumerable<string> tokens, StreamSet streamSet)
    {
        Tokens = tokens.ToArray();
        StreamSet = streamSet;
    }

    public StreamSet StreamSet { get; set; }

    public async Task<int> ExecuteAsync()
    {
        var result = await ExecuteInternalAsync();
        StreamSet.Close();
        return result;
    }

    public async ValueTask DisposeAsync()
    {
        await StreamSet.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    protected abstract Task<int> ExecuteInternalAsync();
}