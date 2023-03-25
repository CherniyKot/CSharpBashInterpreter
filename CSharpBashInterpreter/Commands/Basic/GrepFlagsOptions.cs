namespace CSharpBashInterpreter.Commands.Basic;

public record GrepFlagsOptions(string Pattern,
    IEnumerable<string> FileNames,
    bool UseWordMatch,
    bool CaseInsensitive,
    int? AdditionalWordMatches);