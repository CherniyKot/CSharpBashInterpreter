﻿using System.IO.Pipelines;
using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Commands.Basic;
using CSharpBashInterpreter.Exceptions;
using CSharpBashInterpreter.Semantics;
using FluentAssertions;

namespace CSharpBashInterpreter.Tests.CommandTests
{
    public class PwdTests
    {
        [Fact]
        public void TestPwd()
        {
            var pwdCommandExecutable = new PwdCommandExecutable(new[] { "pwd" });
            var pipe = new Pipe();
            using (var writer = new StreamWriter(pipe.Writer.AsStream()))
            using (var reader = new StreamReader(pipe.Reader.AsStream()))
            {
                pwdCommandExecutable.OutputStream = writer;
                pwdCommandExecutable.Execute().Result.Should().Be(0);
                reader.ReadToEndAsync().Result.Trim().Should().Be(Directory.GetCurrentDirectory());
            }
        }
    }
}
