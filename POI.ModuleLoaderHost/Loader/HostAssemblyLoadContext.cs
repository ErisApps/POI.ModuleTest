using System.Reflection;
using System.Runtime.Loader;
using POI.Shared.Interfaces;

namespace POI.ModuleLoaderHost.Loader
{
    internal class HostAssemblyLoadContext : AssemblyLoadContext, IUnloadable
    {
        private readonly AssemblyDependencyResolver _dependencyResolver;
        private WeakReference<IModuleInterface>? _moduleEntrypoint;

        public HostAssemblyLoadContext(string modulePath) : base(isCollectible: true)
        {
            _dependencyResolver = new AssemblyDependencyResolver(modulePath);
        }

        public void InitAndTrackEntrypoint(IModuleInterface moduleEntrypoint)
        {
            if (moduleEntrypoint == null)
            {
                throw new ArgumentNullException(nameof(moduleEntrypoint), "Entrypoint should not be null, this would result in faulty unloading");
            }

            _moduleEntrypoint = new WeakReference<IModuleInterface>(moduleEntrypoint);

            moduleEntrypoint.InitializeModule();
        }

        public void RequestUnload()
        {
            if (_moduleEntrypoint == null)
            {
                Console.WriteLine("ModuleEntrypoint not set, can't unload properly.");
                return;
            }

            if (_moduleEntrypoint.TryGetTarget(out var moduleInterface))
            {
                moduleInterface.UnloadModule();
            }
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var assemblyPath = _dependencyResolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                Console.WriteLine($"Loading assembly {assemblyPath} into the HostAssemblyLoadContext");
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }
}