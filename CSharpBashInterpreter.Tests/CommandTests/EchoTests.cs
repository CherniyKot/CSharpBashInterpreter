using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Exceptions;
using CSharpBashInterpreter.Semantics;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.CommandTests;

public class EchoTests
{
    [Fact]
    public void TestEchoOnSingleString()
    {
        var testText = Faker.Lorem.Sentence();

        var echoCommandExecutable = new EchoCommandExecutable(new[] { "echo", testText });
        var pipe = new Pipe();
        using (var writer = new StreamWriter(pipe.Writer.AsStream()))
        using (var reader = new StreamReader(pipe.Reader.AsStream()))
        {
            echoCommandExecutable.OutputStream = writer;
            echoCommandExecutable.Execute().Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Trim().Should().Be(testText);
        }
    }
}
