namespace CSharpBashInterpreter.Utility;

public class ConsoleState
{
    public static ConsoleState GetDefaultConsoleState() => new ConsoleState()
    {
        CurrentDirectory = Directory.GetCurrentDirectory(),
    };

    private string _currentDirectory;

    public string CurrentDirectory
    {
        get => _currentDirectory;
        set => _currentDirectory = Path.GetFullPath(value);
    }
}