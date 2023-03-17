using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Utility;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.CommandTests;

public class LsTests
{
    [Fact]
    public void TestLs()
    {
        var tempFileName = Directory.GetCurrentDirectory();
        var testText = "";

        foreach (var file in Directory.GetFiles(tempFileName)) testText += Path.GetFileName(file) + Environment.NewLine;

        var lsCommandExecutable = new LsCommandExecutable(new[] { "ls" }, new StreamSet());
        var pipe = new Pipe();
        using (var writer = new StreamWriter(pipe.Writer.AsStream()))
        using (var reader = new StreamReader(pipe.Reader.AsStream()))
        {
            lsCommandExecutable.StreamSet.OutputStream = writer;
            lsCommandExecutable.ExecuteAsync().Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Should().Be(testText);
        }
    }

    [Fact]
    public void TestLsWithDir()
    {
        var tempFileName = Directory.GetCurrentDirectory();
        var testText = "";

        foreach (var file in Directory.GetFiles(tempFileName)) testText += Path.GetFileName(file) + Environment.NewLine;

        var lsCommandExecutable = new LsCommandExecutable(new[] { "ls", tempFileName }, new StreamSet());
        var pipe = new Pipe();
        using (var writer = new StreamWriter(pipe.Writer.AsStream()))
        using (var reader = new StreamReader(pipe.Reader.AsStream()))
        {
            lsCommandExecutable.StreamSet.OutputStream = writer;
            lsCommandExecutable.ExecuteAsync().Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Should().Be(testText);
        }
    }
}