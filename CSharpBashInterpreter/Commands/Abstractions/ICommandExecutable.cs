using CSharpBashInterpreter.Utility;

namespace CSharpBashInterpreter.Commands.Abstractions;

/// <summary>
/// Command runtime executable with built arguments
/// </summary>
public interface ICommandExecutable
{
    /// <summary>
    /// Run command action
    /// </summary>
    /// <param name="streams">Stream set for command</param>
    Task<int> ExecuteAsync(StreamSet streams);
}