﻿using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Utility;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.CommandTests;

public class WcTests
{
    [Fact]
    public void TestWcOnSingleFile()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            var testText = "Hello world\nHow are you?";
            var testResult = $"2 5 24 {tempFileName}";
            File.WriteAllText(tempFileName, testText);

            var wcCommandExecutable = new WcCommandExecutable(new[] { "wc", tempFileName });
            var pipe = new Pipe();
            using var reader = new StreamReader(pipe.Reader.AsStream());
            var streams = new StreamSet
            {
                OutputStream = pipe.Writer.AsStream(),
            };
            wcCommandExecutable.ExecuteAsync(streams, ConsoleState.GetDefaultConsoleState()).Result.Should().Be(0);
            var result = reader.ReadToEndAsync().Result.TrimEnd();
            result.TrimEnd().Should().Be(testResult);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }
}