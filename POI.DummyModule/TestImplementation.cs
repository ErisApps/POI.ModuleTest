using POI.Shared.Interfaces;

namespace POI.DummyModule
{
    public class TestImplementation : IModuleInterface
    {
        public void InitializeModule()
        {
            Console.WriteLine($"Hii from the lovely {nameof(DummyModule)} ^^");
        }

        public void UnloadModule()
        {
            Console.WriteLine($"Bye bye, {nameof(DummyModule)} will miss you :c");
        }
    }
}