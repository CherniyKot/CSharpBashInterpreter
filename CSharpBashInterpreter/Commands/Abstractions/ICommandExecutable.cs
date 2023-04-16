using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
///     Command runtime executable with built arguments
/// </summary>
public interface ICommandExecutable : IAsyncDisposable
{
    /// <summary>
    ///     Run command action
    /// </summary>
    /// <param name="streams">Stream set for command</param>
    /// <param name="consoleState">Current console parameters</param>
    /// <returns></returns>
    Task<int> ExecuteAsync(StreamSet streams, ConsoleState consoleState);
}