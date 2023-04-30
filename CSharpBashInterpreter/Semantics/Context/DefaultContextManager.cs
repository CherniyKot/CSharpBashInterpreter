using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Semantics.Context;

/// <summary>
/// Default realization of manager with parsing by Regex
/// </summary>
public class DefaultContextManager : IContextManager
{
    public IContext GenerateContext()
    {
        return new DefaultContext();
    }

    public string SubstituteVariablesInText(string input, IContext context)
    {
        return RegularExpressions.SimpleQuoteTokenizerRegex()
            .Replace(input, match => match.Groups[1].Success
                ? MatchQuotes(match.Groups[1].Value, context)
                : match.Groups[2].Success
                    ? $"\"{MatchQuotes(match.Groups[2].Value, context)}\""
                    : match.Value);
    }

    private static string MatchQuotes(string input, IContext context) =>
        RegularExpressions.EnvironmentVariablesRegex()
            .Replace(input, quoteMatch =>
            {
                var group = quoteMatch.Groups[1].Success
                    ? quoteMatch.Groups[1]
                    : quoteMatch.Groups[2];
                return context.EnvironmentVariables.TryGetValue(group.Value, out var value)
                    ? value
                    : "";
            });
}