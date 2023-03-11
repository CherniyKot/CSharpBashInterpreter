using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Semantics;

/// <summary>
/// Implementation of <see cref="ITokenizer"/>, who emulating
/// equal logic of parsing like bash
/// </summary>
public class SpaceTokenizer : ITokenizer
{
    public string[] Tokenize(string input) =>
        RegularExpressions.QuoteTokenizerRegex()
        .Matches(input)
        .SelectMany(match => match.Groups[0].Value[0] switch
        {
            '"' => new[] { match.Groups[3].Value },
            '\'' => new[] { match.Groups[4].Value },
            _ => match.Groups[1].Success
                ? new[] { match.Groups[1].Value, "=", match.Groups[2].Value }
                : new[] { match.Groups[0].Value }
        }).ToArray();

}