using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Utility;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.CommandTests;

public class PwdTests
{
    [Fact]
    public void TestPwd()
    {
        var pwdCommandExecutable = new PwdCommandExecutable(new[] { "pwd" }, new StreamSet());
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        pwdCommandExecutable.StreamSet.OutputStream = pipe.Writer.AsStream();
        pwdCommandExecutable.ExecuteAsync().Result.Should().Be(0);
        reader.ReadToEndAsync().Result.Trim().Should().Be(Directory.GetCurrentDirectory());
    }
}