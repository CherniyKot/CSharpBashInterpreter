namespace CSharpBashInterpreter.Semantics;

public interface IContextManager
{
    IContext GenerateContext();
    string SubstituteVariablesInText(string input, IContext context);
}