using System.Diagnostics.CodeAnalysis;
using CommandLine;
using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

public class GrepCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "grep";
    public override ICommandExecutable Build(IEnumerable<string> tokens)
    {
        return new GrepCommandExecutable(tokens, ParseFlagsFromTokens);
    }

    private static bool ParseFlagsFromTokens(IEnumerable<string> tokens,
        [NotNullWhen(true)] out GrepFlagsOptions? onsuccess)
    {
        onsuccess = null;
        var result = Parser.Default.ParseArguments<GrepConsoleFlags>(tokens);

        if (result is not Parsed<GrepConsoleFlags> flagsResult)
            return false;

        onsuccess = flagsResult.Value.ToOptions();
        return true;
    }

    private class GrepConsoleFlags
    {
        [Value(0)]
        public string Pattern { get; set; } = string.Empty;
        [Value(1, Min=1)]
        public IEnumerable<string> FileNames { get; set; } = ArraySegment<string>.Empty;

        [Option('w')]
        public bool UseFullWord { get; set; } = false;
        [Option('i')]
        public bool CaseInsensitive { get; set; } = false;
        [Option('A')]
        public int? AdditionalWordMatches { get; set; } = null;

        public GrepFlagsOptions ToOptions() =>
            new(Pattern,
                FileNames,
                UseFullWord,
                CaseInsensitive,
                AdditionalWordMatches);
    }
}