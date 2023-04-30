﻿using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Executable for bash echo command
/// Takes list of 2 tokens: "echo" and string
/// sends string to the output stream
/// </summary>
public class EchoCommandExecutable : BaseCommandExecutable
{
    public EchoCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    {
    }

    protected override async Task<int> ExecuteInternalAsync(StreamSet streamSet)
    {
        try
        {
            var concatArgs = string.Join(' ', Tokens.Skip(1));
            await using var outputStream = new StreamWriter(streamSet.OutputStream);
            await outputStream.WriteLineAsync(concatArgs);
            await outputStream.FlushAsync();
        }
        catch (Exception e)
        {
            await using var errorStream = new StreamWriter(streamSet.ErrorStream);
            await errorStream.WriteLineAsync(e.Message);
            await errorStream.FlushAsync();
            return 1;
        }

        return 0;
    }
}