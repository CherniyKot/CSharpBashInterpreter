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

        foreach (var file in Directory.GetDirectories(tempFileName))
            testText += Path.GetFileName(file) + Path.DirectorySeparatorChar + Environment.NewLine;
        foreach (var file in Directory.GetFiles(tempFileName))
            testText += Path.GetFileName(file) + Environment.NewLine;

        var lsCommandExecutable = new LsCommandExecutable(new[] { "ls" });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        lsCommandExecutable.ExecuteAsync(streams, ConsoleState.GetDefaultConsoleState()).Result.Should().Be(0);
        reader.ReadToEndAsync().Result.Should().Be(testText);
    }

    [Fact]
    public void TestLsWithDirectory()
    {
        var tempDirInfo = Directory.CreateDirectory(Path.GetRandomFileName());
        var testText = "";

        foreach (var file in Directory.GetDirectories(tempDirInfo.FullName))
            testText += Path.GetFileName(file) + Path.DirectorySeparatorChar + Environment.NewLine;
        foreach (var file in Directory.GetFiles(tempDirInfo.FullName))
            testText += Path.GetFileName(file) + Environment.NewLine;

        var lsCommandExecutable = new LsCommandExecutable(new[] { "ls", tempDirInfo.Name });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        var result = lsCommandExecutable.ExecuteAsync(streams, ConsoleState.GetDefaultConsoleState()).Result;
        Directory.Delete(tempDirInfo.FullName);
        result.Should().Be(0);
        reader.ReadToEndAsync().Result.Should().Be(testText);
    }
    
    [Fact]
    public void TestLsWithFile()
    {
        var tempFilePath = new FileInfo(Path.GetTempFileName());
        File.Create(tempFilePath.FullName).Dispose();
        
        var lsCommandExecutable = new LsCommandExecutable(new[] { "ls", tempFilePath.FullName });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        var exitCode = lsCommandExecutable.ExecuteAsync(streams, ConsoleState.GetDefaultConsoleState()).Result;
        File.Delete(tempFilePath.FullName);
        exitCode.Should().Be(0);
        reader.ReadToEndAsync().Result.TrimEnd().Should().Be(tempFilePath.FullName);
    }
    
    [Fact]
    public void TestLsWithoutFile()
    {
        var tempFileName = Path.GetRandomFileName();
        
        var lsCommandExecutable = new LsCommandExecutable(new[] { "ls", tempFileName });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        lsCommandExecutable.ExecuteAsync(streams, ConsoleState.GetDefaultConsoleState()).Result.Should().Be(1);
    }
}
