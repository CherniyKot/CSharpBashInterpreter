using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Exceptions;
using CSharpBashInterpreter.Semantics.Context;
using CSharpBashInterpreter.Semantics.Parsing;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.SemanticsTests;

public class ParserTests
{
    [Fact]
    public void TestParserOnEmptyCommands()
    {
        var context = new DefaultContext();
        var parser = new DefaultCommandsParser
        {
            MetaCommands = Array.Empty<IMetaCommandRepresentation>(),
            Commands = Array.Empty<ICommandRepresentation>()
        };

        var tokens = new[] { "" };

        Assert.Throws<ParseException>(() => parser.Parse(tokens, context));
    }

    [Fact]
    public void TestParserOnCatCommands()
    {
        var context = new DefaultContext();
        var parser = new DefaultCommandsParser
        {
            MetaCommands = Array.Empty<IMetaCommandRepresentation>(),
            Commands = new ICommandRepresentation[]
            {
                new CatCommandRepresentation()
            }
        };

        var file = Path.GetTempFileName();
        var tokens = new[] { "cat", file };

        try
        {
            var result = parser.Parse(tokens, context);
            result.Should().BeOfType<CatCommandExecutable>();
        }
        finally
        {
            File.Delete(file);
        }
    }
}