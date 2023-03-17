using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Meta;

/// <summary>
///     Used for provide substitution of input string
/// </summary>
public class ContextSetCommandRepresentation : IMetaCommandRepresentation
{
    public bool CanBeParsed(IEnumerable<string> data)
    {
        var enumerable = data as string[] ?? data.ToArray();
        return enumerable is [_, "=", _];
    }

    public ICommandExecutable Build(IEnumerable<string> input, IContext context, ICommandParser parser,
        StreamSet streamSet)
    {
        return new ContextSetCommandExecutable(input, context, streamSet);
    }
}