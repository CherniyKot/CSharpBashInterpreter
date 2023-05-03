using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Utility;
using Faker;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.CommandTests;

public class GrepTests
{
    [Fact]
    public void GrepTest1()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            var testText = String.Join(Environment.NewLine, Lorem.Paragraphs(5));
            File.WriteAllText(tempFileName, testText);

            var grepCommandExecutable = new GrepCommandRepresentation().Build(new[] { "grep", testText.Split().First(), tempFileName });
            var pipe = new Pipe();
            using var reader = new StreamReader(pipe.Reader.AsStream());
            var streams = new StreamSet
            {
                OutputStream = pipe.Writer.AsStream(),
            };
            grepCommandExecutable.ExecuteAsync(streams).Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Should().StartWith($"1:{testText.Split(Environment.NewLine).First()}");
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    [Fact]
    public void GrepTest2()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            var testParagraphs = Lorem.Paragraphs(5).ToList();
            var testText = String.Join(Environment.NewLine, testParagraphs);
            File.WriteAllText(tempFileName, testText);

            var grepCommandExecutable = new GrepCommandRepresentation()
                .Build(new[] { "grep", testParagraphs[2].Split().First(), tempFileName });
            var pipe = new Pipe();
            using var reader = new StreamReader(pipe.Reader.AsStream());
            var streams = new StreamSet
            {
                OutputStream = pipe.Writer.AsStream(),
            };
            grepCommandExecutable.ExecuteAsync(streams).Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Should().Contain($"3:{testParagraphs[2]}");
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    [Fact]
    public void GrepTestIFlag()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            var testWord = Lorem.GetFirstWord();
            var testText = string.Join(Environment.NewLine,
                testWord,
                testWord.ToUpper(),
                testWord.ToLower(),
                char.ToUpper(testWord[0]) + new string(testWord.Skip(1).ToArray()));
            File.WriteAllText(tempFileName, testText);

            var grepCommandExecutable = new GrepCommandRepresentation()
                .Build(new[] { "grep", "-i", testWord.ToUpper(), tempFileName });
            var pipe = new Pipe();
            using var reader = new StreamReader(pipe.Reader.AsStream());
            var streams = new StreamSet
            {
                OutputStream = pipe.Writer.AsStream(),
            };
            grepCommandExecutable.ExecuteAsync(streams).Result.Should().Be(0);
            reader.ReadToEndAsync().Result.Should().ContainAll(testWord, testWord.ToLower(), testWord.ToUpper());
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    [Fact]
    public void GrepTestWFlag()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            var testWord = Lorem.GetFirstWord();
            var testText = string.Join(Environment.NewLine, testWord, testWord+testWord+testWord, testWord+testWord);
            File.WriteAllText(tempFileName, testText);

            var grepCommandExecutable = new GrepCommandRepresentation()
                .Build(new[] { "grep", "-w", testWord, tempFileName });
            var pipe = new Pipe();
            using var reader = new StreamReader(pipe.Reader.AsStream());
            var streams = new StreamSet
            {
                OutputStream = pipe.Writer.AsStream(),
            };
            grepCommandExecutable.ExecuteAsync(streams).Result.Should().Be(0);
            var result = reader.ReadToEndAsync().Result;
            result.Should().NotContainAny(testWord + testWord + testWord, testWord + testWord);
            result.Should().Contain(testWord);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    [Fact]
    public void GrepTestAFlag()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            var testWord = Lorem.GetFirstWord();
            var testText = string.Join(Environment.NewLine,
                testWord,
                string.Concat(Enumerable.Repeat(testWord.ToUpper(),10)),
                string.Concat(testWord, testWord, testWord),
                string.Concat(testWord, testWord)
            );
            File.WriteAllText(tempFileName, testText);

            var grepCommandExecutable = new GrepCommandRepresentation()
                .Build(new[] { "grep", "-A 3", testWord, tempFileName });
            var pipe = new Pipe();
            using var reader = new StreamReader(pipe.Reader.AsStream());
            var streams = new StreamSet
            {
                OutputStream = pipe.Writer.AsStream(),
            };
            grepCommandExecutable.ExecuteAsync(streams).Result.Should().Be(0);
            var result =reader.ReadToEndAsync().Result;
            result.Should().NotContainAny("5:","6:","7:");
            result.Should().ContainAll("1:","2:","3:","4:");
            result.Should().NotMatch("12:*12:");
            result.Should().NotMatch("13:*13:");
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }
}