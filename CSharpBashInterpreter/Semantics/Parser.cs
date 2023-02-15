using CSharpBashInterpreter.Commands;
using CSharpBashInterpreter.Commands.BasicCommands;
using CSharpBashInterpreter.Commands.Factory;
using CSharpBashInterpreter.Commands.MetaCommands;
using CSharpBashInterpreter.Exceptions;

namespace CSharpBashInterpreter.Semantics
{
    internal static class Parser
    {
        public static List<ICommandFactory<MetaTerminalCommand>> MetaCommands = new();
        public static List<ICommandFactory<BasicTerminalCommand>> Commands = new();

        public static AbstractTerminalCommand Parse(IEnumerable<string> tokens)
        {
            var tokenList = tokens.ToList();
            foreach (var metaCommand in MetaCommands)
            {
                if (metaCommand.CanBeParsed(tokenList))
                {
                    return metaCommand.Instantiate(tokenList);
                }
            }

            foreach (var command in Commands)
            {
                if (command.CanBeParsed(tokenList))
                {
                    return command.Instantiate(tokenList.Skip(1));
                }
            }
            //add filesystem query here

            throw new ParseException(tokenList);
        }
    }
}
