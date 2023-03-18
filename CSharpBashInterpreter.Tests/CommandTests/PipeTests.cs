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
            var testText = "Helloworld";
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

            var pipe = new Pipe();

            StreamSet ss = new StreamSet();
            ss.OutputStream = pipe.Writer.AsStream();

            var pipeCommandExecutable = new PipeCommandExecutable(
                new[] { "echo", testText, "|", "cat" }, "|", new DefaultContext(), commandsParser, ss);

            using var reader = new StreamReader(pipe.Reader.AsStream());

            pipeCommandExecutable.StreamSet.OutputStream = pipe.Writer.AsStream();
            pipeCommandExecutable.ExecuteAsync().Result.Should().Be(0);
            reader.ReadToEndAsync().Result.TrimEnd().Should().Be(testResult);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }
}
