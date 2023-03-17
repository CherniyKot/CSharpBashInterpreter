using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Meta;

/// <summary>
///     Used for substitute environment variable to context
/// </summary>
public class ContextSetCommandExecutable : BaseCommandExecutable
{
    private readonly IContext _context;

    public ContextSetCommandExecutable(IEnumerable<string> tokens, IContext context, StreamSet streamSet) : base(tokens,
        streamSet)
    {
        _context = context;
    }

    protected override Task<int> ExecuteInternalAsync()
    {
        _context.EnvironmentVariables[Tokens[0]] = Tokens[2];
        return Task.FromResult(0);
    }
}