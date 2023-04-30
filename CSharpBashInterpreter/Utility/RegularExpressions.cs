using System.Text.RegularExpressions;

namespace CSharpBashInterpreter.Utility;

internal static partial class RegularExpressions
{
    [GeneratedRegex("""(\w+)=(.+)|[^\s"']+|"([^"]*)"|'([^']*)'""")]
    public static partial Regex QuoteTokenizerRegex();

    [GeneratedRegex("""([^\s"']+)|"([^"]*)"|'([^']*)'""")]
    public static partial Regex SimpleQuoteTokenizerRegex();

    [GeneratedRegex("""\${(\w+)}|\$(\w+)""")]
    public static partial Regex EnvironmentVariablesRegex();
}