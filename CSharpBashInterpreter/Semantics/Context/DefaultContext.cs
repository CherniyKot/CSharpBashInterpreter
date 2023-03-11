using System.Collections.Concurrent;
using CSharpBashInterpreter.Semantics.Abstractions;

namespace CSharpBashInterpreter.Semantics.Context;

public class DefaultContext : IContext
{
    public IDictionary<string, string> EnvironmentVariables { get; } = new ConcurrentDictionary<string, string>();
}