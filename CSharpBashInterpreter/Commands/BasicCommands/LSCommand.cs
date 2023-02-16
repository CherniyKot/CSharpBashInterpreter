using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBashInterpreter.Commands.BasicCommands
{
    internal class LSCommand : BasicTerminalCommand, IParsable
    {
        private const string Name = "ls";

        protected override async Task _Run()
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            for (int i = 0; i < files.Length; i++)
            {
                await OutputStream.WriteAsync(Path.GetFileName(files[i]) + '\n');
                await OutputStream.FlushAsync();
            }
        }

        public static bool CanBeParsed(IEnumerable<string> tokens)
        {
            return tokens.First() == Name;
        }
    }
}
