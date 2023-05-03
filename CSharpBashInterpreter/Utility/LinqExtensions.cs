namespace CSharpBashInterpreter.Utility;

public static class LinqExtensions
{
    public static string AggregateToString(this IEnumerable<string> strings) => string.Join("\n", strings);
}