namespace CSharpBashInterpreter.Semantics.Abstractions;

/// <summary>
///     Provide elements used in context of executions
/// </summary>
public interface IContext
{
    public interface IVariablesHandler : IDictionary<string, string>
    {
        public IDictionary<string, string> Internals { get; }
    }
    
    IVariablesHandler EnvironmentVariables { get; }
}