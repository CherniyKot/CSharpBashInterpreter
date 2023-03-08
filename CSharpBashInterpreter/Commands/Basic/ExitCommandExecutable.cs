﻿namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Executable for exit command
/// exits the process with specified exit code (0 by default)
/// </summary>
public class ExitCommandExecutable : BaseCommandExecutable
{
    public ExitCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    public override async Task<int> Execute()
    {
        Environment.Exit(Tokens.Length > 1 ? int.Parse(Tokens[1]) : 0);
        return 0;
    }
}