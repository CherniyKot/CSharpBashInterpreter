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
        var cdCommandExecutable = new CdCommandExecutable(new[] { "cd", ".." });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var consoleState = ConsoleState.GetDefaultConsoleState();
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        cdCommandExecutable.ExecuteAsync(streams, consoleState).Result.Should().Be(0);
        reader.ReadToEndAsync().Result.Trim().Should().Be("");
        
        var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
        consoleState.CurrentDirectory.Should().Be(parentDirectory?.FullName ?? Directory.GetCurrentDirectory());
    }
    
    [Fact]
    public void TestCdForManyOnePoints()
    {
        var cdCommandExecutable = new CdCommandExecutable(new[] { "cd", "././././." });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var consoleState = ConsoleState.GetDefaultConsoleState();
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        cdCommandExecutable.ExecuteAsync(streams, consoleState).Result.Should().Be(0);
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
        
        var cdCommandExecutable = new CdCommandExecutable(new[] { "cd", $"../../{firstParent.Name}" });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var consoleState = ConsoleState.GetDefaultConsoleState();
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        cdCommandExecutable.ExecuteAsync(streams, consoleState).Result.Should().Be(0);
        reader.ReadToEndAsync().Result.Trim().Should().Be("");
        consoleState.CurrentDirectory.Should().Be(nextDirectory.FullName);
    }
    
    [Fact]
    public void TestCdWithoutArguments()
    {
        var cdCommandExecutable = new CdCommandExecutable(new[] { "cd" });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var consoleState = ConsoleState.GetDefaultConsoleState();
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        cdCommandExecutable.ExecuteAsync(streams, consoleState).Result.Should().Be(0);
        reader.ReadToEndAsync().Result.Trim().Should().Be("");
        
        consoleState.CurrentDirectory.Should().Be(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    }
    
    [Fact]
    public void TestCdWithAbsolutePath()
    {
        var absoluteDirectory = new DirectoryInfo("/");

        var cdCommandExecutable = new CdCommandExecutable(new[] { "cd", "/" });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var consoleState = ConsoleState.GetDefaultConsoleState();
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        
        var result = cdCommandExecutable.ExecuteAsync(streams, consoleState).Result;
        result.Should().Be(0);
        reader.ReadToEndAsync().Result.Should().Be("");
        
        consoleState.CurrentDirectory.Should().Be(absoluteDirectory.FullName);
    }
    
    [Fact]
    public void TestCdWithNonExistent()
    {
        var cdCommandExecutable = new CdCommandExecutable(new[] { "cd", Path.GetRandomFileName() });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        
        cdCommandExecutable.ExecuteAsync(streams, ConsoleState.GetDefaultConsoleState()).Result.Should().Be(1);
    }
    
    [Fact]
    public void TestCdWithFile()
    {
        var tempFilePath = new FileInfo(Path.GetTempFileName());
        File.Create(tempFilePath.FullName).Dispose();
        
        var cdCommandExecutable = new CdCommandExecutable(new[] { "cd", Path.GetRandomFileName() });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        
        var exitCode = cdCommandExecutable.ExecuteAsync(streams, ConsoleState.GetDefaultConsoleState()).Result;
        File.Delete(tempFilePath.FullName);
        exitCode.Should().Be(1);
    }
    
    [Fact]
    public void TestCdWithManyArguments()
    {
        var cdCommandExecutable = new CdCommandExecutable(new[] { "cd", "temp", "temp" });
        var pipe = new Pipe();
        using var reader = new StreamReader(pipe.Reader.AsStream());
        var streams = new StreamSet
        {
            OutputStream = pipe.Writer.AsStream(),
        };
        
        cdCommandExecutable.ExecuteAsync(streams, ConsoleState.GetDefaultConsoleState()).Result.Should().Be(1);
    }
}