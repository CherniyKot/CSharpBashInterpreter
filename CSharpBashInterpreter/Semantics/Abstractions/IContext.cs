namespace CSharpBashInterpreter.Semantics.Abstractions;

/// <summary>
///     Provide elements used in context of executions
/// </summary>
public interface IContext
{
    /// <summary>
    ///     Provide interface for collection of environment variables
    /// </summary>
    IDictionary<string, string> EnvironmentVariables { get; }
}