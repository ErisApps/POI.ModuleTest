using System.Reflection;
using System.Runtime.Loader;
using DryIoc;
using POI.Shared.Interfaces;

namespace POI.ModuleLoaderHost.Loader
{
    internal class HostAssemblyLoadContext : AssemblyLoadContext, IUnloadable
    {
	    private readonly WeakReference _depContainerWeakRef;
        private readonly AssemblyDependencyResolver _dependencyResolver;

        private WeakReference<IModuleInterface>? _moduleEntrypoint;

        // ReSharper disable once SuggestBaseTypeForParameterInConstructor
        public HostAssemblyLoadContext(IContainer container, string modulePath) : base(isCollectible: true)
        {
	        _depContainerWeakRef = new WeakReference(container);
            _dependencyResolver = new AssemblyDependencyResolver(modulePath);
        }

        public void InitAndTrackEntrypoint(IModuleInterface moduleEntrypoint)
        {
            if (moduleEntrypoint == null)
            {
                throw new ArgumentNullException(nameof(moduleEntrypoint), "Entrypoint should not be null, this would result in faulty (un)loading");
            }

            _moduleEntrypoint = new WeakReference<IModuleInterface>(moduleEntrypoint);

            if (!_depContainerWeakRef.IsAlive || _depContainerWeakRef.Target is not IContainer container)
			{
				throw new InvalidOperationException("Container is not alive, this would result in faulty (un)loading");
			}

            moduleEntrypoint.InitializeModule(container);
        }

        public void RequestUnload()
        {
            if (_moduleEntrypoint == null)
            {
                Console.WriteLine("ModuleEntrypoint not set, can't unload properly.");
                return;
            }

            if (!_depContainerWeakRef.IsAlive || _depContainerWeakRef.Target is not IContainer container)
            {
	            throw new InvalidOperationException("Container is not alive, can't unload properly.");
            }

            if (_moduleEntrypoint.TryGetTarget(out var moduleInterface))
            {
                moduleInterface.UnloadModule(container);
            }

            Unload();
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