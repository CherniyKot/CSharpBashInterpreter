namespace CSharpBashInterpreter.Utility;

public class ConsoleState
{
    public static ConsoleState GetDefaultConsoleState() => new()
    {
        CurrentDirectory = Directory.GetCurrentDirectory(),
    };

    private string _currentDirectory = string.Empty;

    public string CurrentDirectory
    {
        get => _currentDirectory;
        set => _currentDirectory = Path.GetFullPath(value);
    }

    public string ConvertPath(string argPath)
    {
        string resultPath = argPath.StartsWith(Path.DirectorySeparatorChar) ||
                            argPath.StartsWith(Path.AltDirectorySeparatorChar)
            ? argPath : Path.Combine(CurrentDirectory, argPath);
    
        if (!Path.Exists(resultPath))
        {
            var maybeFullPath = Path.GetFullPath(argPath);
            if (!Path.Exists(maybeFullPath))
                throw new ArgumentException($"Could not find path {resultPath}");
            resultPath = maybeFullPath;
        }

        return resultPath;
    }
}