using System.Text.RegularExpressions;

namespace CSharpBashInterpreter.Semantics;

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