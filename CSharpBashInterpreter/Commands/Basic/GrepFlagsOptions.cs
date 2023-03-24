namespace CSharpBashInterpreter.Commands.Basic;

public record GrepFlagsOptions(string Pattern,
    IEnumerable<string> FileNames,
    bool UseFullWord,
    bool CaseInsensitive,
    int? AdditionalWordMatches);