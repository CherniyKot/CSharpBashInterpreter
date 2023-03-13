using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics.Abstractions;

namespace CSharpBashInterpreter.Commands.Meta;

/// <summary>
/// Used for provide substitution of input string
/// </summary>
public class ContextSetCommandRepresentation : IMetaCommandRepresentation
{
    public bool CanBeParsed(IEnumerable<string> data)
    {
        var enumerable = data as string[] ?? data.ToArray();
        return enumerable is [_, "=", _];
    }

    public ICommandExecutable Build(IEnumerable<string> input, IContext context, ICommandParser parser)
    {
        return new ContextSetCommandExecutable(input, context);
    }
}