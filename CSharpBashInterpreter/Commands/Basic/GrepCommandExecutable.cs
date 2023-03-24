using System.Text.RegularExpressions;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

public class GrepCommandExecutable : BaseCommandExecutable
{
    private readonly TryParseFunction<GrepFlagsOptions> _parser;

    public GrepCommandExecutable(IEnumerable<string> tokens, TryParseFunction<GrepFlagsOptions> parser) : base(tokens)
    {
        _parser = parser;
    }

    protected override async Task<int> ExecuteInternalAsync()
    {
        try
        {
            if (!_parser(Tokens.Skip(1), out var flags))
                return 1;

            var files = await ReadFilesFromSystem(flags.FileNames);
            var matches = FormatMatchingLines(flags, files);

            await using var output = new StreamWriter(StreamSet.OutputStream);
            await output.WriteLineAsync(matches);
            await output.FlushAsync();
        }
        catch (Exception exception)
        {
            await using var output = new StreamWriter(StreamSet.ErrorStream);
            await output.WriteLineAsync(exception.Message);
            await output.FlushAsync();

            return 2;
        }

        return 0;
    }

    private static async Task<IList<FileData>> ReadFilesFromSystem(IEnumerable<string> fileNames) =>
        await Task.WhenAll(fileNames.Select(async fileName =>
        {
            var lines1 = await File.ReadAllLinesAsync(fileName);
            return new FileData(fileName, lines1.Select((str, i) => new Line(i + 1, str)));
        }));

    private static string FormatMatchingLines(GrepFlagsOptions flags, ICollection<FileData> files) =>
        files.SelectMany(file =>
            {
                var isMatchPredicate = CreateMatchPredicate(flags);
                return file.Content
                    .Where(isMatchPredicate)
                    .Select(line => files.Count > 1
                        ? $"{file.FileName}:{line.Index}:{line.Text}"
                        : $"{line.Index}:{line.Text}")
                    .TakeWhile((_, index) =>
                        !flags.AdditionalWordMatches.HasValue || index < flags.AdditionalWordMatches);
            })
            .AggregateToString();

    private static Func<Line, bool> CreateMatchPredicate(GrepFlagsOptions flags)
    {
        var matchPattern = flags.UseFullWord ? $"^{flags.Pattern}$" : flags.Pattern;
        var options = flags.CaseInsensitive ? RegexOptions.IgnoreCase : RegexOptions.None;
        var regex = new Regex(matchPattern, options);
        return line => regex.IsMatch(line.Text);
    }

    private record struct Line(int Index, string Text);
    private record FileData(string FileName, IEnumerable<Line> Content);
}
