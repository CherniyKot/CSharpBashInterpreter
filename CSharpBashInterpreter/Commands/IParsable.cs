namespace CSharpBashInterpreter;

public interface IParsable
{
    public static abstract bool CanBeParsed(IEnumerable<string> tokens);
}