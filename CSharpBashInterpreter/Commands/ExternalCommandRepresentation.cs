﻿using CSharpBashInterpreter.Commands.Abstractions;

namespace CSharpBashInterpreter.Commands;

/// <summary>
/// Representation for external commands (calls to OS processes)
/// </summary>
public class ExternalCommandRepresentation: ICommandRepresentation
{
    public string Name => "";

    public ICommandExecutable Build(IEnumerable<string> tokens)
    {
        return new ExternalCommandExecutable(tokens);
    }

    public bool CanBeParsed(IEnumerable<string> data) => true;
}