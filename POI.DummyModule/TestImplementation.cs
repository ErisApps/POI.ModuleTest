using DryIoc;
using POI.Shared.Interfaces;

namespace POI.DummyModule
{
	public class TestImplementation : IModuleInterface
	{
		public void InitializeModule(IContainer container)
		{
			Console.WriteLine($"Hii from the lovely {nameof(DummyModule)} ^^");
		}

		public void UnloadModule(IContainer container)
		{
			Console.WriteLine($"Bye bye, {nameof(DummyModule)} will miss you :c");
		}
	}
}