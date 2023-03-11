using System.Collections.Concurrent;

namespace CSharpBashInterpreter.Semantics;

public class DefaultContext : IContext
{
    public IDictionary<string, string> EnvironmentVariables { get; } = new ConcurrentDictionary<string, string>();
}