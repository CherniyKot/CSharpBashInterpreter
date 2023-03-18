namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
///     Provide information and logic of basic calling command
/// </summary>
public interface ICommandRepresentation : IAbstractCommandRepresentation
{
    /// <summary>
    ///     Build command runtime with applied arguments from tokens
    /// </summary>
    ICommandExecutable Build(IEnumerable<string> tokens);
}