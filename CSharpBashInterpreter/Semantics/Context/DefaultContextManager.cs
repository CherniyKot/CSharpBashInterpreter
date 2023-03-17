using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Semantics.Context;

/// <summary>
///     Default realization of manager with parsing by Regex
/// </summary>
public class DefaultContextManager : IContextManager
{
    public IContext GenerateContext()
    {
        return new DefaultContext();
    }

    public string SubstituteVariablesInText(string input, IContext context)
    {
        return RegularExpressions.EnvironmentVariablesRegex()
            .Replace(input, match =>
            {
                var group = match.Groups[1].Success ? match.Groups[1] : match.Groups[2];
                return context.EnvironmentVariables.TryGetValue(group.Value, out var value)
                    ? value
                    : "";
            });
    }
}