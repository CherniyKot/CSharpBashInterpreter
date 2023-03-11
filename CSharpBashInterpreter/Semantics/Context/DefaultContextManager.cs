using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Semantics;

public class DefaultContextManager : IContextManager
{
    public IContext GenerateContext() => new DefaultContext();

    public string SubstituteVariablesInText(string input, IContext context) =>
        RegularExpressions.EnvironmentVariablesRegex()
            .Replace(input, match =>
            {
                var group = match.Groups[1].Success ? match.Groups[1] : match.Groups[2];
                return context.EnvironmentVariables.TryGetValue(group.Value, out var value)
                    ? value
                    : "";
            });
}