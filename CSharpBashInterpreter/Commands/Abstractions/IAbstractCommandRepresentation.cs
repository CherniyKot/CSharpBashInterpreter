namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
/// Base abstraction of command information
/// </summary>
public interface IAbstractCommandRepresentation
{
    /// <summary>
    /// Check if input tokens can be processed by command
    /// </summary>
    /// <param name="data">Parsed tokens from console</param>
    bool CanBeParsed(IEnumerable<string> data);
}