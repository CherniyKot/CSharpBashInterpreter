using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Basic;
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
            using (var writer = new StreamWriter(pipe.Writer.AsStream()))
            using (var reader = new StreamReader(pipe.Reader.AsStream()))
            {
                wcCommandExecutable.OutputStream = writer;
                wcCommandExecutable.ExecuteAsync().Result.Should().Be(0);
                reader.ReadToEndAsync().Result.TrimEnd().Should().Be(testResult);
            }
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }
}