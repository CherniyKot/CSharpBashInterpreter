namespace CSharpBashInterpreter.Semantics;

class SpaceTokenParser : ITokenParser
{
    public List<string> Tokenize(string input) => input.Split().ToList();
}