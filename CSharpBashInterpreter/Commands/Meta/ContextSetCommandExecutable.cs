using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Commands.Meta;

public class ContextSetCommandExecutable : BaseCommandExecutable
{
    private readonly IContext _context;

    public ContextSetCommandExecutable(IEnumerable<string> tokens, IContext context) : base(tokens)
    {
        _context = context;
    }

    protected override Task<int> ExecuteInternalAsync()
    {
        _context.EnvironmentVariables[Tokens[0]] = Tokens[2];
        return Task.FromResult(0);
    }
}