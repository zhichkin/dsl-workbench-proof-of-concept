using System;

namespace OneCSharp.MVVM
{
    public interface IModule
    {
        IShell Shell { get; }
        void Initialize(IShell shell);
        T GetService<T>();
        T GetProvider<T>();
        T GetController<T>();
        IController GetController(Type type);
    }
}