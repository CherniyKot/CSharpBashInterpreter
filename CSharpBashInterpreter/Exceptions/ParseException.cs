namespace CSharpBashInterpreter.Exceptions;

public class ParseException : Exception
{
    private readonly string _tokens;

    public ParseException(IEnumerable<string> tokens)
    {
        _tokens = string.Join(' ', tokens);
    }

    public override string Message => $"Can't parse string \"{_tokens}\".";
}