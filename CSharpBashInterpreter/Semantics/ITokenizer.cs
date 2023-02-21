namespace CSharpBashInterpreter.Semantics;

public interface ITokenizer
{
    string[] Tokenize(string input);
}