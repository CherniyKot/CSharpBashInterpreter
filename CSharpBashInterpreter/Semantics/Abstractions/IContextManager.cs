namespace CSharpBashInterpreter.Semantics.Abstractions;

public interface IContextManager
{
    IContext GenerateContext();
    string SubstituteVariablesInText(string input, IContext context);
}