using FluentAssertions;
using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Basic;

namespace CSharpBashInterpreter.Tests.CommandTests
{
    public class LsTests
    {
        [Fact]
        public void TestLs()
        {
            var tempFileName = System.IO.Directory.GetCurrentDirectory();
            var testText = "";

            foreach (var file in Directory.GetFiles(tempFileName))
            {
                testText += Path.GetFileName(file) + '\n';
            }

            var lsCommandExecutable = new LsCommandExecutable(new[] { "ls" });
            var pipe = new Pipe();
            using (var writer = new StreamWriter(pipe.Writer.AsStream()))
            using (var reader = new StreamReader(pipe.Reader.AsStream()))
            {
                lsCommandExecutable.OutputStream = writer;
                lsCommandExecutable.Execute().Result.Should().Be(0);
                reader.ReadToEndAsync().Result.Should().Be(testText);
            }
        }

        [Fact]
        public void TestLsWithDir()
        {
            var tempFileName = System.IO.Directory.GetCurrentDirectory();
            var testText = "";

            foreach (var file in Directory.GetFiles(tempFileName))
            {
                testText += Path.GetFileName(file) + '\n';
            }

            var lsCommandExecutable = new LsCommandExecutable(new[] { "ls", tempFileName });
            var pipe = new Pipe();
            using (var writer = new StreamWriter(pipe.Writer.AsStream()))
            using (var reader = new StreamReader(pipe.Reader.AsStream()))
            {
                lsCommandExecutable.OutputStream = writer;
                lsCommandExecutable.Execute().Result.Should().Be(0);
                reader.ReadToEndAsync().Result.Should().Be(testText);
            }
        }
    }
}
