using System;

namespace OneCSharp.MVVM
{
    public interface IModule
    {
        IShell Shell { get; }
        void Initialize(IShell shell);
        IController GetController<T>();
        IController GetController(Type type);
    }
}