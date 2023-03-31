using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Utility;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.CommandTests;

public class CdTests
{
    [Fact]
    public void TestCdForParentDirectory()
    {
        var pwdCommandExecutable = new CdCommandExecutable(new[] { "cd", ".." });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var consoleState = ConsoleState.GetDefaultConsoleState();
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        pwdCommandExecutable.ExecuteAsync(streams, consoleState).Result.Should().Be(0);
        reader.ReadToEndAsync().Result.Trim().Should().Be("");
        
        var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
        consoleState.CurrentDirectory.Should().Be(parentDirectory?.FullName ?? Directory.GetCurrentDirectory());
    }
    
    [Fact]
    public void TestForManyOnePoints()
    {
        var pwdCommandExecutable = new CdCommandExecutable(new[] { "cd", "././././." });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var consoleState = ConsoleState.GetDefaultConsoleState();
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        pwdCommandExecutable.ExecuteAsync(streams, consoleState).Result.Should().Be(0);
        reader.ReadToEndAsync().Result.Trim().Should().Be("");
        
        consoleState.CurrentDirectory.Should().Be(Directory.GetCurrentDirectory());
    }
    
    [Fact]
    public void TestCdComplex()
    {
        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        var firstParent = Directory.GetParent(Directory.GetCurrentDirectory()) ?? currentDirectory;
        var secondParent = firstParent.Parent ?? firstParent;
        var nextDirectory = secondParent.EnumerateDirectories().FirstOrDefault((info) => info.Name == firstParent.Name)
                            ?? currentDirectory;
        
        var pwdCommandExecutable = new CdCommandExecutable(new[] { "cd", $"../../{firstParent.Name}" });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var consoleState = ConsoleState.GetDefaultConsoleState();
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        pwdCommandExecutable.ExecuteAsync(streams, consoleState).Result.Should().Be(0);
        reader.ReadToEndAsync().Result.Trim().Should().Be("");
        consoleState.CurrentDirectory.Should().Be(nextDirectory.FullName);
    }
}