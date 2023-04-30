namespace CSharpBashInterpreter.Semantics.Abstractions;

/// <summary>
/// Provide elements used in context of executions
/// </summary>
public interface IContext
{
    /// <summary>
    /// Provide interface for collection of environment variables
    /// </summary>
    IDictionary<string, string> EnvironmentVariables { get; }

    /// <summary>
    /// Provide substitution of environment variables from context to input string
    /// </summary>
    /// <param name="input">Console input with added or not substitutions</param>
    /// <returns>Unchanged input string, if no substitution or changed input string</returns>
    string SubstituteVariablesInText(string input);
}