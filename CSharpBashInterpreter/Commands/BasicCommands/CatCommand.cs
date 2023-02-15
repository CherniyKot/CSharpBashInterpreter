namespace CSharpBashInterpreter.Commands.BasicCommands
{
    internal class CatCommand : BasicTerminalCommand, IParsable
    {
        private const string Name = "cat";

        private char[] buf = new char[256];
        protected override async Task _Run()
        {
            if (args.Any())
            {
                foreach (var fileName in args)
                {
                    try
                    {
                        using (var fileStream = File.OpenText(fileName))
                        {
                            while (!fileStream.EndOfStream)
                            {
                                int bytesRead = await fileStream.ReadAsync(buf, 0, 256);
                                await OutputStream.WriteAsync(buf, 0, bytesRead);
                                await OutputStream.FlushAsync();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        await ErrorStream.WriteLineAsync(e.Message);
                        await ErrorStream.FlushAsync();
                    }
                }

                await OutputStream.DisposeAsync();
            }
            else
            {
                {
                    try
                    {
                        while (InputStream.BaseStream.CanRead)
                        {
                            int bytesRead = await InputStream.ReadAsync(buf, 0, 256);
                            await OutputStream.WriteAsync(buf, 0, bytesRead);
                            await OutputStream.FlushAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        await ErrorStream.WriteLineAsync(e.Message);
                        await ErrorStream.FlushAsync();
                    }
                }
                await OutputStream.DisposeAsync();
            }
        }

        public static bool CanBeParsed(IEnumerable<string> tokens)
        {
            return tokens.First() == Name;
        }
    }
}
