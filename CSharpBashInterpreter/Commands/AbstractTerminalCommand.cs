using CSharpBashInterpreter.Commands.MetaCommands.Utility;
using CSharpBashInterpreter.Exceptions;

namespace CSharpBashInterpreter.Commands
{
    public abstract class AbstractTerminalCommand
    {
        public StreamReader InputStream { get; set; } = new StreamReader(new InterruptableConsoleStream());
        public StreamWriter OutputStream { get; set; } = new StreamWriter(Console.OpenStandardOutput());
        public StreamWriter ErrorStream { get; set; } = new StreamWriter(Console.OpenStandardError());

        public Task Run()
        {
            if (!IsInitialized)
            {
                throw new NotInitializedException();
            }

            return _Run();
        }

        protected abstract Task _Run();

        public abstract void Initialize(IEnumerable<string> tokens);
        public bool IsInitialized { get; protected set; } = false;
    }
}
