using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.External;

/// <summary>
///     Representation for external commands (calls to OS processes)
/// </summary>
public class ExternalCommandRepresentation : IExternalCommandRepresentation
{
    public ICommandExecutable Build(IEnumerable<string> tokens, IContext context, StreamSet streamSet)
    {
        return new ExternalCommandExecutable(tokens, context, streamSet);
    }

    public bool CanBeParsed(IEnumerable<string> data)
    {
        return true;
    }
}