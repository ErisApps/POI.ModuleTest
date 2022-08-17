using DryIoc;

namespace POI.Shared.Interfaces
{
    public interface IModuleInterface
    {
        void InitializeModule(IContainer container);
        void UnloadModule(IContainer container);
    }
}