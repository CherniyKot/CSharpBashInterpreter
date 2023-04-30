﻿namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Representation for bash echo command
/// </summary>
class EchoCommandRepresentation : BaseCommandRepresentation
{
    public override string Name => "echo";

    public override BaseCommandExecutable Build(IEnumerable<string> tokens)
    {
        return new EchoCommandExecutable(tokens);
    }
}
