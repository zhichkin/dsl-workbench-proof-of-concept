using OneCSharp.AST.UI;
using OneCSharp.Integrator.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;

namespace OneCSharp.Integrator.Module
{
    public sealed class IntegratorModule : IModule
    {
        private readonly Dictionary<Type, IController> _controllers = new Dictionary<Type, IController>();
        public IntegratorModule() { }
        public IController GetController<T>()
        {
            return _controllers[typeof(T)];
        }
        public IController GetController(Type type)
        {
            return _controllers[type];
        }
        public IShell Shell { get; private set; }
        public void Initialize(IShell shell)
        {
            Shell = shell ?? throw new ArgumentNullException(nameof(shell));
            ConfigureController();
            ConfigureView();
            ConfigureLanguage();
        }
        private void ConfigureController()
        {
            _controllers.Add(typeof(IntegratorModule), new IntegratorModuleController(Shell));
        }
        private void ConfigureView()
        {
            IController controller = GetController<IntegratorModule>();
            controller.BuildTreeNode(null, out TreeNodeViewModel mainNode);
            Shell.AddTreeNode(mainNode);
        }
        private void ConfigureLanguage()
        {
            SyntaxTreeController.Current.RegisterConceptLayout(
                typeof(CreateIntegrationNodeConcept),
                (IConceptLayout)Activator.CreateInstance(typeof(CreateIntegrationNodeLayout)));
        }
    }
}