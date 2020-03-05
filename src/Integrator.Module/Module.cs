using OneCSharp.MVVM;
using System;
using System.Collections.Generic;

namespace OneCSharp.Integrator.Module
{
    public sealed class Module : IModule
    {
        private readonly Dictionary<Type, IController> _controllers = new Dictionary<Type, IController>();
        public Module() { }
        public IController GetController<T>()
        {
            return _controllers[typeof(T)];
        }
        public IController GetController(Type type)
        {
            return _controllers[type];
        }
        public void Persist(object entity)
        {
            throw new NotImplementedException();
        }
        public IShell Shell { get; private set; }
        public void Initialize(IShell shell)
        {
            Shell = shell ?? throw new ArgumentNullException(nameof(shell));
            _controllers.Add(typeof(Module), new ModuleController(Shell));
            ConfigureView();
        }
        private void ConfigureView()
        {
            IController controller = GetController<Module>();
            controller.BuildTreeNode(null, out TreeNodeViewModel mainNode);
            Shell.AddTreeNode(mainNode);
        }
    }
}