namespace CSharpBashInterpreter.Semantics.Abstractions;

public interface IContext
{
    IDictionary<string, string> EnvironmentVariables { get; }
}