using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Utility;
using Faker;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.CommandTests;

public class CatTests
{
    [Fact]
    [Fact(Skip = "")]
    public void TestCatOnSingleFile()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            var testText = Lorem.Paragraph();
            File.WriteAllText(tempFileName, testText);

            var catCommandExecutable = new CatCommandExecutable(new[] { "cat", tempFileName });
            var pipe = new Pipe();
            using var reader = new StreamReader(pipe.Reader.AsStream());
            var streams = new StreamSet
            {
                OutputStream = pipe.Writer.AsStream(),
            };
            catCommandExecutable.ExecuteAsync(streams).Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Should().Be(testText);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    [Fact(Skip = "")]
    public void TestCatOnMultipleFiles()
    {
        var tempFiles = new List<string> { "cat" };
        var testTexts = new List<string>();
        for (var i = 0; i < 10; i++)
        {
            tempFiles.Add(Path.GetTempFileName());
            testTexts.Add(Lorem.Paragraph());
            File.WriteAllText(tempFiles.Last(), testTexts.Last());
        }

        try
        {
            var catCommandExecutable = new CatCommandExecutable(tempFiles);
            var pipe = new Pipe();

            using var reader = new StreamReader(pipe.Reader.AsStream());
            var streams = new StreamSet
            {
                OutputStream = pipe.Writer.AsStream(),
            };
            catCommandExecutable.ExecuteAsync(streams).Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Should().Be(string.Join("", testTexts));
        }
        finally
        {
            tempFiles.Skip(1).ToList().ForEach(File.Delete);
        }
    }

    [Fact(Skip = "Console stream is tricky and powerful")]
    public void TestCatOnInputStream()
    {
        var testText = Lorem.Paragraph();
        var catCommandExecutable = new CatCommandExecutable(new[] { "cat" });
        var pipeInput = new PipeWrapper();
        var pipeOutput = new PipeWrapper();

        var streams = new StreamSet
        {
            OutputStream = pipeOutput.WriterStream,
            InputStream = pipeInput.ReaderStream
        };

        using var writerInput = new StreamWriter(pipeInput.WriterStream);
        using var readerOutput = new StreamReader(pipeOutput.ReaderStream);
        writerInput.WriteLine(testText);
        writerInput.Flush();
        var task = catCommandExecutable.ExecuteAsync(streams);
        writerInput.Close();
        readerOutput.ReadToEndAsync().Result.Should().Be(testText);
        task.Result.Should().Be(0);
    }
}