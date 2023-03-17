namespace CSharpBashInterpreter.Semantics.Abstractions;

/// <summary>
///     Manager with functionality to creating and using context dependency logic
/// </summary>
public interface IContextManager
{
    /// <summary>
    ///     Create empty context for execution
    /// </summary>
    /// <returns></returns>
    IContext GenerateContext();

    /// <summary>
    ///     Provide substitution of environment variables from context to input string
    /// </summary>
    /// <param name="input">Console input with added or not substitutions</param>
    /// <param name="context">Operation context</param>
    /// <returns>Unchanged input string, if no substitution or changed input string</returns>
    string SubstituteVariablesInText(string input, IContext context);
}