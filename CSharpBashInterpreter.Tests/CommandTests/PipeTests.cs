using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Commands.External;
using CSharpBashInterpreter.Commands.Meta;
using CSharpBashInterpreter.Semantics.Context;
using CSharpBashInterpreter.Semantics.Parsing;
using CSharpBashInterpreter.Utility;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.CommandTests;

public class PipeTests
{
    [Fact]
    public void TestPipe()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            var testText = "Hello world";
            var testResult = testText;
            File.WriteAllText(tempFileName, testText);

            var commandsParser = new DefaultCommandsParser
            {
                Commands = new ICommandRepresentation[]
                {
                    new CatCommandRepresentation(),
                    new EchoCommandRepresentation(),
                },
                            MetaCommands = new IMetaCommandRepresentation[]
                {
                    new PipeCommandRepresentation()
                },
                ExternalCommandRepresentation = new ExternalCommandRepresentation()
            };

            var wcCommandExecutable = new PipeCommandExecutable(
                new[] { "cat", tempFileName, "|", "echo" }, "|", new DefaultContext(), commandsParser,  new StreamSet());
            var pipe = new Pipe();
            using var reader = new StreamReader(pipe.Reader.AsStream());
            wcCommandExecutable.StreamSet.OutputStream = pipe.Writer.AsStream();
            wcCommandExecutable.ExecuteAsync().Result.Should().Be(0);
            reader.ReadToEndAsync().Result.TrimEnd().Should().Be(testResult);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }
}
