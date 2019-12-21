using OneCSharp.Core;

namespace OneCSharp.MVVM
{
    public interface IModule
    {
        void Initialize(IShell shell);
        IController GetController<T>();
        void Persist(Entity model);
    }
}