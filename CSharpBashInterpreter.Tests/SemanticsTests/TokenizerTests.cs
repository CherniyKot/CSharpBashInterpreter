using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Semantics.Parsing;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.SemanticsTests;

public class TokenizerTests
{
    private readonly ITokenizer _tokenizer;

    public TokenizerTests()
    {
        _tokenizer = new SpaceTokenizer();
    }

    [Fact]
    public void TestTokenizerOnEmptyString()
    {
        var result = _tokenizer.Tokenize("");

        result.Should().BeEmpty();
    }

    [Fact]
    public void TestTokenizerOnOneTokenString()
    {
        var result = _tokenizer.Tokenize("hello");

        result.Should().ContainSingle(x => x == "hello");
        result.Length.Should().Be(1);
    }

    [Fact]
    public void TestTokenizerOnTwoTokenString()
    {
        var result = _tokenizer.Tokenize("hello my world");

        result.Should().BeEquivalentTo("hello", "my", "world");
        result.Length.Should().Be(3);
    }

    [Fact]
    public void TestTokenizerOnQuotesTokenString()
    {
        var result = _tokenizer.Tokenize("""hello "my dear" world""");
        var result2 = _tokenizer.Tokenize("""hello 'my dear' world""");

        result.Should().BeEquivalentTo("hello", "my dear", "world");
        result2.Should().BeEquivalentTo("hello", "my dear", "world");
    }

    [Fact]
    public void TestTokenizerQuotesWithEnvironmentSubstitute()
    {
        var result = _tokenizer.Tokenize("""hello=world""");
        var result2 = _tokenizer.Tokenize("""hello=my dear "world" """);

        result.Should().BeEquivalentTo("hello", "=", "world");
        result2.Should().BeEquivalentTo("hello", "=", """my dear "world" """);
    }
}