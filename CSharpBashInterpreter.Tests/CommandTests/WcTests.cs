using System.IO.Pipelines;
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

            var wcCommandExecutable = new WcCommandExecutable(new[] { "wc", tempFileName }, new StreamSet());
            var pipe = new Pipe();
            using var reader = new StreamReader(pipe.Reader.AsStream());
            wcCommandExecutable.StreamSet.OutputStream = pipe.Writer.AsStream();
            wcCommandExecutable.ExecuteAsync().Result.Should().Be(0);
            reader.ReadToEndAsync().Result.TrimEnd().Should().Be(testResult);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }
}