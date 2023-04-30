using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Semantics.Context;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.SemanticsTests;

public class ContextTests
{
    private readonly IContextManager _contextManager = new DefaultContextManager();

    [Fact]
    public void TestContextToSetAndGetVariables()
    {
        var context = _contextManager.GenerateContext();
        var key = "a";
        var value = "a";
        context.EnvironmentVariables.Add(key, value);
        context.EnvironmentVariables.TryGetValue(key, out var result).Should().BeTrue();
        result.Should().BeEquivalentTo(value);
    }

    [Fact]
    public void TestNewContextFromManager()
    {
        var context = _contextManager.GenerateContext();
        var key = "a";
        var value = "b";
        context.EnvironmentVariables.Add(key, value);

        var newContext = _contextManager.GenerateContext();
        newContext.EnvironmentVariables.Count.Should().Be(0);
        newContext.Should().NotBe(context);
    }

    [Fact]
    public void TestSubstitution()
    {
        var context = _contextManager.GenerateContext();
        var key = "a";
        var value = "b";
        context.EnvironmentVariables.Add(key, value);

        var textDefault = "$a=100";
        var textWithBrackets = "${a}=100";
        var textWithSingleQuotes = "'${a}=100'";
        var textWithDoubleQuotes = "\"${a}=100\"";

        var newText1 = _contextManager.SubstituteVariablesInText(textDefault, context);
        var newText2 = _contextManager.SubstituteVariablesInText(textWithBrackets, context);
        var newText3 = _contextManager.SubstituteVariablesInText(textWithDoubleQuotes, context);
        var unchangedText = _contextManager.SubstituteVariablesInText(textWithSingleQuotes, context);

        newText1.Should().BeEquivalentTo(newText2).And.BeEquivalentTo("b=100");
        newText3.Should().BeEquivalentTo("\"b=100\"");
        unchangedText.Should().BeEquivalentTo(textWithSingleQuotes);
    }
}