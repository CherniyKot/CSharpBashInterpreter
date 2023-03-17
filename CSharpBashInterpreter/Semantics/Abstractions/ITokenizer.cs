namespace CSharpBashInterpreter.Semantics.Abstractions;

/// <summary>
///     Provide logic of console input tokenizing
/// </summary>
public interface ITokenizer
{
    /// <summary>
    ///     Split console input string to tokens
    /// </summary>
    string[] Tokenize(string input);
}