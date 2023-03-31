using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Utility;
using Faker;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.CommandTests;

public class EchoTests
{
    [Fact]
    public void TestEchoOnSingleString()
    {
        var testText = Lorem.Sentence();

        var echoCommandExecutable = new EchoCommandExecutable(new[] { "echo", testText });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        echoCommandExecutable.ExecuteAsync(streams, ConsoleState.GetDefaultConsoleState()).Result.Should().Be(0);
        reader.ReadToEndAsync().Result.Trim().Should().Be(testText);
    }
}