namespace CSharpBashInterpreter.Commands.Abstractions;

public interface ICommandExecutable
{
    StreamReader InputStream { get; set; }
    StreamWriter OutputStream { get; set; }
    StreamWriter ErrorStream { get; set; }

    Task Execute();
}