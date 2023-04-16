using CSharpBashInterpreter.Commands.Meta;
using CSharpBashInterpreter.Semantics.Abstractions;
using CSharpBashInterpreter.Semantics.Context;
using CSharpBashInterpreter.Utility;
using FluentAssertions;
using Moq;

namespace CSharpBashInterpreter.Tests.CommandTests;

public class SubstituteTests
{
    private readonly IContext _context = new DefaultContext();
    private readonly ContextSetCommandRepresentation _representation = new();

    [Fact]
    public void TestCanBeParsed()
    {
        var key = "a";
        var value = "a";

        var tokens = new[] { key, "=", value };
        _representation.CanBeParsed(tokens).Should().BeTrue();
    }

    [Fact]
    public async Task ContextBuildCommand()
    {
        var key = "a";
        var value = "a";

        var tokens = new[] { key, "=", value };
        _context.EnvironmentVariables.Add(key, value);

        _representation.CanBeParsed(tokens).Should().BeTrue();
        var command = _representation.Build(tokens, _context, Mock.Of<ICommandParser>());
        var code = await command.ExecuteAsync(new StreamSet(), ConsoleState.GetDefaultConsoleState());
        code.Should().Be(0);

        _context.EnvironmentVariables.Should().ContainKey(key);
        _context.EnvironmentVariables[key].Should().BeEquivalentTo(value);
    }
}