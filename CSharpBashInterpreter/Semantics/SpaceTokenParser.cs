namespace CSharpBashInterpreter.Semantics;

class SpaceTokenParser : ITokenParser
{
    public string[] Tokenize(string input) => input.Split().ToArray();
}