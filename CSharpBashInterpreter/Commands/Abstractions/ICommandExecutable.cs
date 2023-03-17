using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
///     Command runtime executable with built arguments
/// </summary>
public interface ICommandExecutable : IAsyncDisposable
{
    StreamSet StreamSet { get; set; }

    /// <summary>
    ///     Run command action
    /// </summary>
    /// <returns></returns>
    Task<int> ExecuteAsync();
}