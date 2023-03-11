﻿using CSharpBashInterpreter.Commands.Abstractions;
using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Commands.External;

/// <summary>
/// Representation for external commands (calls to OS processes)
/// </summary>
public class ExternalCommandRepresentation: IExternalCommandRepresentation
{
    public ICommandExecutable Build(IEnumerable<string> tokens, IContext context)
        => new ExternalCommandExecutable(tokens, context);

    public bool CanBeParsed(IEnumerable<string> data) => true;
}