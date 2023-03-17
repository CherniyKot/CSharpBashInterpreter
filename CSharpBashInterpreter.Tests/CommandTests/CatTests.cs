using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Utility;
using Faker;
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
            var testText = Lorem.Paragraph();
            File.WriteAllText(tempFileName, testText);

            var catCommandExecutable = new CatCommandExecutable(new[] { "cat", tempFileName }, new StreamSet());
            var pipe = new Pipe();
            using var reader = new StreamReader(pipe.Reader.AsStream());
            catCommandExecutable.StreamSet.OutputStream = pipe.Writer.AsStream();
            catCommandExecutable.ExecuteAsync().Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Should().Be(testText);
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
            testTexts.Add(Lorem.Paragraph());
            File.WriteAllText(tempFiles.Last(), testTexts.Last());
        }

        try
        {
            var catCommandExecutable = new CatCommandExecutable(tempFiles, new StreamSet());
            var pipe = new Pipe();

            using var reader = new StreamReader(pipe.Reader.AsStream());
            catCommandExecutable.StreamSet.OutputStream = pipe.Writer.AsStream();
            catCommandExecutable.ExecuteAsync().Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Should().Be(string.Join("", testTexts));
        }
        finally
        {
            tempFiles.ForEach(File.Delete);
        }
    }

    [Fact(Skip = "Stream reading is not cancellable yet")]
    public void TestCatOnInputStream()
    {
        var testText = Lorem.Paragraph();
        var catCommandExecutable = new CatCommandExecutable(new[] { "cat" }, new StreamSet());
        var pipeInput = new Pipe();
        var pipeOutput = new Pipe();
        
        catCommandExecutable.StreamSet.InputStream = pipeInput.Reader.AsStream();
        catCommandExecutable.StreamSet.OutputStream = pipeOutput.Writer.AsStream();
        catCommandExecutable.ExecuteAsync().Result.Should().Be(0);
        
        using var writerInput = new StreamWriter(pipeInput.Writer.AsStream());
        using var readerOutput = new StreamReader(pipeOutput.Reader.AsStream());
        writerInput.WriteLine(testText);
        writerInput.Close();
        readerOutput.ReadToEndAsync().Result.Should().Be(testText);
    }
}