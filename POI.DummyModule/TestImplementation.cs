using System.Diagnostics;
using DryIoc;
using POI.DummyModule.Services;
using POI.DummyModule.Services.Interfaces;
using POI.Shared.Interfaces;

namespace POI.DummyModule
{
	public class TestImplementation : IModuleInterface
	{
		public void InitializeModule(IContainer container)
		{
			Console.WriteLine($"Hii from the lovely {nameof(DummyModule)} ^^");

			container.Register<ITestService, TestService>();

			Debugger.Break();
		}

		public void UnloadModule(IContainer container)
		{
			container.Unregister<ITestService>();

			Console.WriteLine($"Bye bye, {nameof(DummyModule)} will miss you :c");

			Debugger.Break();
		}
	}
}