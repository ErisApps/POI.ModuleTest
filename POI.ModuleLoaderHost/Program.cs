using POI.ModuleLoaderHost.Loader;

namespace POI.ModuleLoaderHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var pluginFullPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\POI.DummyModule\\bin\\Debug\\net6.0\\POI.DummyModule.dll"));
            var moduleLoader = new ModuleLoader();

            do
            {
                switch (ListenToCommand())
                {
                    case KeyboardCommand.CommandLoad:
                        moduleLoader.Load(pluginFullPath);
                        break;
                    case KeyboardCommand.CommandUnload:
                        moduleLoader.Unload(pluginFullPath);
                        break;
                    default:
                        Console.WriteLine("Unknown command, exiting...");
                        return;
                }
            } while (true);
        }

        private static KeyboardCommand ListenToCommand()
        {
            Console.WriteLine();
            Console.Write("Next command: ");
            if (Enum.TryParse(Console.In.ReadLine(), true, out KeyboardCommand command))
            {
                return command;
            }

            return (KeyboardCommand)int.MaxValue;
        }

        private enum KeyboardCommand
        {
            CommandLoad,
            CommandUnload
        }
    }
}