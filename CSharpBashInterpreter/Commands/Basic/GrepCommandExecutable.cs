using System.Text.RegularExpressions;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

public class GrepCommandExecutable : BaseCommandExecutable
{
    private readonly Func<IEnumerable<string>, GrepFlagsOptions> _parser;

    public GrepCommandExecutable(IEnumerable<string> tokens,
        Func<IEnumerable<string>, GrepFlagsOptions> parser)
        : base(tokens)
    {
        _parser = parser;
    }

    protected override async Task<int> ExecuteInternalAsync(StreamSet streamSet)
    {
        try
        {
            var flags = _parser(Tokens.Skip(1));

            string matches;
            if (flags.FileNames.Any())
            {
                var files = await ReadFilesFromSystem(flags.FileNames);
                matches = MatchAndFormatFromFiles(files, flags);
            }
            else
                matches = await MatchAndFormatFromStream(streamSet.InputStream, flags);

            await using var output = new StreamWriter(streamSet.OutputStream);
            await output.WriteLineAsync(matches);
            await output.FlushAsync();
        }
        catch (Exception exception) when(exception is IOException or NotSupportedException or ArgumentException)
        {
            await using var output = new StreamWriter(streamSet.ErrorStream);
            await output.WriteLineAsync(exception.Message);
            await output.FlushAsync();

            return exception is ArgumentException ? 2 : 1;
        }

        return 0;
    }

    private static async Task<IList<FileData>> ReadFilesFromSystem(IEnumerable<string> fileNames) =>
        await Task.WhenAll(fileNames.Select(async fileName =>
        {
            var lines = await File.ReadAllLinesAsync(fileName);
            return new FileData(fileName, lines.Select((str, i) => new Line(i + 1, str)));
        }));

    private static string MatchAndFormatFromFiles(ICollection<FileData> files, GrepFlagsOptions flags) =>
        files
            .SelectMany(file => GetMatchesContent(file.Content, flags)
                .Select(line => files.Count > 1
                    ? $"{file.FileName}:{line.Index}:{line.Text}"
                    : $"{line.Index}:{line.Text}"))
            .AggregateToString();

    private static async Task<string> MatchAndFormatFromStream(Stream input, GrepFlagsOptions flags)
    {
        var index = 1;
        var content = new List<Line>();
        using var reader = new StreamReader(input);
        while (input.CanRead)
        {
            var line = await reader.ReadLineAsync() ?? "";
            content.Add(new Line(index++, line));
        }
        return GetMatchesContent(content, flags)
            .Select(line => $"{line.Index}:{line.Text}")
            .AggregateToString();
    }

    private static IEnumerable<Line> GetMatchesContent(IEnumerable<Line> collection, GrepFlagsOptions flags)
    {
        var isMatchPredicate = CreateMatchPredicate(flags);
        if (!flags.AdditionalWordMatches.HasValue)
            return collection.Where(isMatchPredicate);


        var content = collection.Select((line, index) => (line, index)).ToArray();
        var matches = content.Where(tuple => isMatchPredicate(tuple.line)).ToArray();

        var result = new List<Line>();
        var afterCount = flags.AdditionalWordMatches.Value;
        for (var i = 0; i < matches.Length; i++)
        {
            var (line, index) = matches[i];
            var count = i + 1 != matches.Length
                ? Math.Min(afterCount, matches[i + 1].index - index - 1)
                : afterCount;
            result.Add(line);
            result.AddRange(content.Skip(index + 1).Take(count).Select(x => x.line));
        }

        return result;
    }

    private static Func<Line, bool> CreateMatchPredicate(GrepFlagsOptions flags)
    {
        var matchPattern = flags.UseWordMatch ? $"\\b{flags.Pattern}\\b" : flags.Pattern;
        var options = flags.CaseInsensitive ? RegexOptions.IgnoreCase : RegexOptions.None;
        var regex = new Regex(matchPattern, options);
        return line => regex.IsMatch(line.Text);
    }

    private record struct Line(int Index, string Text);
    private record FileData(string FileName, IEnumerable<Line> Content);
}
