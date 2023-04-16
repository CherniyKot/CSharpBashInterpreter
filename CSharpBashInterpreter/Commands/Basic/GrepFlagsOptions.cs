namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
///     Record from arguments for bash grep command
///     <param name="UseWordMatch">(-w) search only for the whole word</param>
///     <param name="CaseInsensitive">(-i) case-insensitive search</param>
///     <param name="AdditionalWordMatches">(-A) how many lines should be printed after the match</param>
/// </summary>
public record GrepFlagsOptions(string Pattern,
    IEnumerable<string> FileNames,
    bool UseWordMatch,
    bool CaseInsensitive,
    int? AdditionalWordMatches);