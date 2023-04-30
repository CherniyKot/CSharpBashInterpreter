using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Basic;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.CommandTests;

public class CatTests
{
    [Fact]
    public void TestCatOnSingleFile()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            var testText = Faker.Lorem.Paragraph();
            File.WriteAllText(tempFileName, testText);

            var catCommandExecutable = new CatCommandExecutable(new[] { "cat", tempFileName });
            var pipe = new Pipe();
            using var writer = new StreamWriter(pipe.Writer.AsStream());
            using var reader = new StreamReader(pipe.Reader.AsStream());
            catCommandExecutable.OutputStream = writer;
            catCommandExecutable.ExecuteAsync().Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Should().Be(testText + Environment.NewLine);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    [Fact]
    public void TestCatOnMultipleFiles()
    {
        var tempFiles = new List<string> { "cat" };
        var testTexts = new List<string>();
        for (var i = 0; i < 10; i++)
        {
            tempFiles.Add(Path.GetTempFileName());
            testTexts.Add(Faker.Lorem.Paragraph());
            File.WriteAllText(tempFiles.Last(), testTexts.Last());
        }

        try
        {
            var catCommandExecutable = new CatCommandExecutable(tempFiles);
            var pipe = new Pipe();

            using var writer = new StreamWriter(pipe.Writer.AsStream());
            using var reader = new StreamReader(pipe.Reader.AsStream());
            catCommandExecutable.OutputStream = writer;
            catCommandExecutable.ExecuteAsync().Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Should().Be(string.Join("", testTexts) + Environment.NewLine);
        }
        finally
        {
            tempFiles.ForEach(File.Delete);
        }
    }

    [Fact(Skip = "Stream reading is not cancellable yet")]
    public void TestCatOnInputStream()
    {
        var testText = Faker.Lorem.Paragraph();
        var catCommandExecutable = new CatCommandExecutable(new[] { "cat" });
        var pipeInput = new Pipe();
        var pipeOutput = new Pipe();

        using var writerInput = new StreamWriter(pipeInput.Writer.AsStream());
        using var readerInput = new StreamReader(pipeInput.Reader.AsStream());
        using var writerOutput = new StreamWriter(pipeOutput.Writer.AsStream());
        using var readerOutput = new StreamReader(pipeOutput.Reader.AsStream());
        catCommandExecutable.InputStream = readerInput;
        catCommandExecutable.OutputStream = writerOutput;
        catCommandExecutable.ExecuteAsync().Result.Should().Be(0);

        writerInput.WriteLine(testText);
        writerInput.Close();
        readerOutput.ReadToEndAsync().Result.Should().Be(testText);
    }
}