using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands;

/// <summary>
/// Representation for external commands (calls to OS processes)
/// </summary>
public class ExternalCommandRepresentation: ICommandRepresentation
{
    public ICommandExecutable Build(IEnumerable<string> tokens) => new ExternalCommandExecutable(tokens);

    public bool CanBeParsed(IEnumerable<string> data) => true;
}