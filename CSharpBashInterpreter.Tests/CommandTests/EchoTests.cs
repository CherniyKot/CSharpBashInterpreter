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

        var echoCommandExecutable = new EchoCommandExecutable(new[] { "echo", testText }, new StreamSet());
        var pipe = new Pipe();
        using (var writer = new StreamWriter(pipe.Writer.AsStream()))
        using (var reader = new StreamReader(pipe.Reader.AsStream()))
        {
            echoCommandExecutable.StreamSet.OutputStream = writer;
            echoCommandExecutable.ExecuteAsync().Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Trim().Should().Be(testText);
        }
    }
}