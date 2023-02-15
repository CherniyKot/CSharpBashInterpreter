using CSharpBashInterpreter.Semantics;

namespace CSharpBashInterpreter.Commands.MetaCommands
{
    internal class Pipe : MetaTerminalCommand, IParsable
    {

        private const string Delimeter = "|";

        private AbstractTerminalCommand left;
        private AbstractTerminalCommand right;

        private System.IO.Pipelines.Pipe pipe;

        public static bool CanBeParsed(IEnumerable<string> tokens)
        {
            return tokens.Contains(Delimeter);
        }

        public override void Initialize(IEnumerable<string> tokens)
        {
            var tokenList = tokens.ToList();
            left = Parser.Parse(tokenList.TakeWhile(t => t != Delimeter));
            right = Parser.Parse(tokenList.SkipWhile(t => t != Delimeter).Skip(1));

            pipe = new System.IO.Pipelines.Pipe();
            var r = pipe.Reader;
            var w = pipe.Writer;
            left.OutputStream = new StreamWriter(w.AsStream());
            right.InputStream = new StreamReader(r.AsStream());

            IsInitialized = true;
        }

        protected override async Task _Run()
        {
            var leftTask = left.Run();
            var rightTask = right.Run();

            await Task.WhenAll(leftTask, rightTask);
        }
    }
}
