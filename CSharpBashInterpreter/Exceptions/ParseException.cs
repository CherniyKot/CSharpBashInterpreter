namespace CSharpBashInterpreter.Exceptions;

public class ParseException : Exception
{
    public string Tokens;

    public ParseException(IEnumerable<string> tokens)
    {
        Tokens = string.Join(' ', tokens);
    }

    public override string Message => $"Can't parse string \"{Tokens}\".";
}