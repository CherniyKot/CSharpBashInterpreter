namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
/// Command runtime executable with built arguments
/// </summary>
public interface ICommandExecutable : IAsyncDisposable
{
    StreamReader InputStream { get; set; }
    StreamWriter OutputStream { get; set; }
    StreamWriter ErrorStream { get; set; }

    /// <summary>
    /// Run command action
    /// </summary>
    /// <returns></returns>
    Task<int> Execute();
}