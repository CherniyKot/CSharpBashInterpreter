namespace CSharpBashInterpreter.Semantics;

class SpaceTokenizer : ITokenizer
{
    public string[] Tokenize(string input) => input.Split().ToArray();
}