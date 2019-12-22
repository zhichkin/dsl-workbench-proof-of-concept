using OneCSharp.Core;
using System;

namespace OneCSharp.MVVM
{
    public interface IModule
    {
        void Initialize(IShell shell);
        IController GetController<T>();
        IController GetController(Type type);
        void Persist(Entity model);
    }
}