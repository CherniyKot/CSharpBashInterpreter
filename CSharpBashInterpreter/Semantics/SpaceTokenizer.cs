using System.Text.RegularExpressions;

namespace CSharpBashInterpreter.Semantics;

/// <summary>
/// Implementation of <see cref="ITokenizer"/>, who emulating
/// equal logic of parsing like bash
/// </summary>
public partial class SpaceTokenizer : ITokenizer
{
    public string[] Tokenize(string input) => QuoteRegexParser()
        .Matches(input)
        .Select(match => match.Groups[0].Value[0] switch
        {
            '"' => match.Groups[1].Value,
            '\'' => match.Groups[2].Value,
            _ => match.Groups[0].Value
        }).ToArray();

    [GeneratedRegex("[^\\s\"']+|\"([^\"]*)\"|'([^']*)'")]
    private static partial Regex QuoteRegexParser();
}