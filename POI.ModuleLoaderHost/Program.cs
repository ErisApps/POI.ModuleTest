using DryIoc;
using POI.ModuleLoaderHost.Loader;

namespace POI.ModuleLoaderHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
	        var container = new Container();
	        container.Register<ModuleLoader>();

            var pluginFullPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../../../../POI.DummyModule/bin/Debug/net6.0/POI.DummyModule.dll"));
            var moduleLoader = container.Resolve<ModuleLoader>();

            do
            {
                switch (ListenToCommand())
                {
                    case KeyboardCommand.Load:
                        moduleLoader.Load(pluginFullPath);
                        break;
                    case KeyboardCommand.Unload:
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
            Load,
            Unload
        }
    }
}