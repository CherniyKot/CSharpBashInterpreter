namespace CSharpBashInterpreter.Semantics;

public interface IContext
{
    IDictionary<string, string> EnvironmentVariables { get; }
}