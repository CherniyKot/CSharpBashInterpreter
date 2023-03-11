using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics.Abstractions;

namespace CSharpBashInterpreter.Commands.Meta;

public class ContextSetCommandRepresentation : IMetaCommandRepresentation
{
    public bool CanBeParsed(IEnumerable<string> data)
    {
        var enumerable = data as string[] ?? data.ToArray();
        return enumerable is [_, "=", _];
    }

    public ICommandExecutable Build(string[] input, IContext context, ICommandParser parser) =>
        new ContextSetCommandExecutable(input, context);
}