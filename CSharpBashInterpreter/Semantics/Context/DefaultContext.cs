using System.Collections.Concurrent;
using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Semantics.Context;

public class DefaultContext : IContext
{
    public IDictionary<string, string> EnvironmentVariables { get; } = new ConcurrentDictionary<string, string>();

    public string SubstituteVariablesInText(string input)
    {
        return RegularExpressions.SimpleQuoteTokenizerRegex()
            .Replace(input, match => match.Groups[1].Success
                ? MatchQuotes(match.Groups[1].Value)
                : match.Groups[2].Success
                    ? $"\"{MatchQuotes(match.Groups[2].Value)}\""
                    : match.Value);
    }

    private string MatchQuotes(string input) =>
        RegularExpressions.EnvironmentVariablesRegex()
            .Replace(input, quoteMatch =>
            {
                var group = quoteMatch.Groups[1].Success
                    ? quoteMatch.Groups[1]
                    : quoteMatch.Groups[2];
                return EnvironmentVariables.TryGetValue(group.Value, out var value)
                    ? value
                    : "";
            });
}