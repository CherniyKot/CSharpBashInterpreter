using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Exceptions;
using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Tests.CommandTests
{
    public class LsTests
    {
        [Fact]
        public void TestCatOnSingleFile()
        {
            var tempFileName = System.IO.Path.GetTempPath();
            try
            {
                File.Create(tempFileName + "/f1.txt");
                File.Create(tempFileName + "/f2.txt");
                File.Create(tempFileName + "/f3.txt");

                var catCommandExecutable = new LsCommandExecutable(new[] { "ls", tempFileName });
                var pipe = new Pipe();
                using (var writer = new StreamWriter(pipe.Writer.AsStream()))
                using (var reader = new StreamReader(pipe.Reader.AsStream()))
                {
                    catCommandExecutable.OutputStream = writer;
                    catCommandExecutable.Execute().Result.Should().Be(0);
                    reader.ReadToEndAsync().Result.Should().Be(testText);
                }
            }
            finally
            {
                File.Delete(tempFileName);
            }
        }
    }
}
