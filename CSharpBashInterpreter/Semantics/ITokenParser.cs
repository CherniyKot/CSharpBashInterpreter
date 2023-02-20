namespace CSharpBashInterpreter.Semantics;

public interface ITokenParser
{
    string[] Tokenize(string input);
}