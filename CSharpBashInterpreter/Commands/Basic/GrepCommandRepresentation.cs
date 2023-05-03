using System.Diagnostics.CodeAnalysis;
using CommandLine;
using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands.Basic;

public class GrepCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "grep";
    public override ICommandExecutable Build(IEnumerable<string> tokens) =>
        new GrepCommandExecutable(tokens, ParseFlagsFromTokens);

    private static GrepFlagsOptions ParseFlagsFromTokens(IEnumerable<string> tokens)
    {
        var result = Parser.Default.ParseArguments<GrepConsoleFlags>(tokens);

        if (result is not Parsed<GrepConsoleFlags> flagsResult || flagsResult.Errors.Any())
            throw new ArgumentException(
                $"Incorrect arguments: {string.Join($";\n", result.Errors.Select(x => x.ToString()))}");

        var value = flagsResult.Value;
        if (string.IsNullOrWhiteSpace(value.Pattern))
            throw new ArgumentException("Pattern can't be empty");

        if (value.AdditionalWordMatches is < 1)
            throw new ArgumentException("Additional words need be great or equal than one");

        return value.ToOptions();
    }

    private class GrepConsoleFlags
    {
        [Value(0, Required = true)]
        public string Pattern { get; set; } = string.Empty;
        [Value(1)]
        public IEnumerable<string> FileNames { get; set; } = ArraySegment<string>.Empty;

        [Option('w')]
        public bool UseWordMatch { get; set; } = false;
        [Option('i')]
        public bool CaseInsensitive { get; set; } = false;
        [Option('A')]
        public int? AdditionalWordMatches { get; set; } = null;

        public GrepFlagsOptions ToOptions() =>
            new(Pattern,
                FileNames,
                UseWordMatch,
                CaseInsensitive,
                AdditionalWordMatches);
    }
}