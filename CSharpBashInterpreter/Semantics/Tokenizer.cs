namespace CSharpBashInterpreter.Semantics
{
    public static class Tokenizer
    {
        public static List<string> Tokenize(string input)
        {
            return input.Split().ToList();
        }
    }
}
