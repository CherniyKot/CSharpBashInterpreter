using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
///     Base class with boilerplate for command executables
/// </summary>
public abstract class BaseCommandExecutable : ICommandExecutable
{
    protected readonly string[] Tokens;

    protected BaseCommandExecutable(IEnumerable<string> tokens)
    {
        Tokens = tokens.ToArray();
    }

    protected StreamSet StreamSet { get; private set; } = null!;

    public async Task<int> ExecuteAsync(StreamSet streams)
    {
        StreamSet = streams;
        var result = await ExecuteInternalAsync();
        streams.Close();
        return result;
    }

    public async ValueTask DisposeAsync()
    {
        await StreamSet.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    protected abstract Task<int> ExecuteInternalAsync();
}