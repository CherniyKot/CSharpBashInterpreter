namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
/// Base abstraction of command information
/// </summary>
/// <typeparam name="TInput"></typeparam>
public interface IAbstractCommandRepresentation
{
    /// <summary>
    /// Check if input tokens can be processed by command
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    bool CanBeParsed(IEnumerable<string> data);
}