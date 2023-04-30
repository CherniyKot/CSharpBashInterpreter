using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Semantics.Context;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.SemanticsTests;

public class ContextTests
{
    private IContext _context = new DefaultContext();

    [Fact]
    public void TestContextToSetAndGetVariables()
    {
        var key = "a";
        var value = "a";
        _context.EnvironmentVariables.Add(key, value);
        _context.EnvironmentVariables.TryGetValue(key, out var result).Should().BeTrue();
        result.Should().BeEquivalentTo(value);
    }

    [Fact]
    public void TestNotStaticNewContext()
    {
        var key = "a";
        var value = "b";
        _context.EnvironmentVariables.Add(key, value);

        var newContext = new DefaultContext();
        newContext.EnvironmentVariables.Count.Should().Be(0);
        newContext.Should().NotBe(_context);
    }

    [Fact]
    public void TestSubstitution()
    {
        var key = "a";
        var value = "b";
        _context.EnvironmentVariables.Add(key, value);

        var textDefault = "$a=100";
        var textWithBrackets = "${a}=100";
        var textWithSingleQuotes = "'${a}=100'";
        var textWithDoubleQuotes = "\"${a}=100\"";

        var newText1 = _context.SubstituteVariablesInText(textDefault);
        var newText2 = _context.SubstituteVariablesInText(textWithBrackets);
        var newText3 = _context.SubstituteVariablesInText(textWithDoubleQuotes);
        var unchangedText = _context.SubstituteVariablesInText(textWithSingleQuotes);

        newText1.Should().BeEquivalentTo(newText2).And.BeEquivalentTo("b=100");
        newText3.Should().BeEquivalentTo("\"b=100\"");
        unchangedText.Should().BeEquivalentTo(textWithSingleQuotes);
    }
}