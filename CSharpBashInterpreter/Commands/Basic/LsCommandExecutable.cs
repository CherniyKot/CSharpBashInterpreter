﻿namespace CSharpBashInterpreter.Commands.Basic;

/// <summary>
/// Executable for bash ls command
/// Does not take arguments
/// Consumes names of files in current directory
/// </summary>
public class LsCommandExecutable : BaseCommandExecutable
{
    public LsCommandExecutable(IEnumerable<string> tokens) : base(tokens)
    { }

    public override async Task<int> Execute()
    {
        try
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (var file in files)
            {
                await OutputStream.WriteAsync(Path.GetFileName(file) + '\n');
                await OutputStream.FlushAsync();
            }
        }
        catch (Exception e)
        {
            await ErrorStream.WriteLineAsync(e.Message);
            await ErrorStream.FlushAsync();
            return 1;
        }

        await OutputStream.DisposeAsync();
        return 0;
    }
}