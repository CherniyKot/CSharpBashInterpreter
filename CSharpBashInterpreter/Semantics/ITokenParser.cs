namespace CSharpBashInterpreter.Semantics;

public interface ITokenParser
{
    List<string> Tokenize(string input);
}