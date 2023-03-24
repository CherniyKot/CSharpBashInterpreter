using System.Diagnostics.CodeAnalysis;

namespace CSharpBashInterpreter.Commands.Abstractions;

public delegate bool TryParseFunction<TResult>(IEnumerable<string> tokens, [NotNullWhen(true)] out TResult? onSuccess);