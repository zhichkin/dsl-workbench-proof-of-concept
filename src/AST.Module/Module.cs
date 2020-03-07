using OneCSharp.AST.Model;
using OneCSharp.AST.UI;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.Module
{
    public sealed class Module : IModule
    {
        #region " String resources "
        public const string ONE_C_SHARP = "ONE-C-SHARP";
        public const string EDIT_WINDOW = "pack://application:,,,/OneCSharp.AST.Module;component/images/EditWindow.png";
        #endregion

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
        public IShell Shell { get; private set; }
        public void Initialize(IShell shell)
        {
            Shell = shell ?? throw new ArgumentNullException(nameof(shell));

            SyntaxTreeManager.RegisterScopeProvider(typeof(LanguageConcept), new AssemblyScopeProvider());

            SyntaxTreeController.Current.RegisterConceptLayout(typeof(ScriptConcept), new ScriptConceptLayout());
            SyntaxTreeController.Current.RegisterConceptLayout(typeof(LanguageConcept), new LanguageConceptLayout());

            Shell.AddMenuItem(new MenuItemViewModel()
            {
                MenuItemIcon = new BitmapImage(new Uri(EDIT_WINDOW)),
                MenuItemHeader = "Add test script",
                MenuItemCommand = new RelayCommand(CreateCodeEditor),
                MenuItemPayload = this
            });
        }
        private void CreateCodeEditor(object parameter)
        {
            //FunctionConcept concept = new FunctionConcept();
            ScriptConcept script = new ScriptConcept();
            CodeEditor editor = new CodeEditor()
            {
                DataContext = SyntaxTreeController.Current.CreateSyntaxNode(null, script)
            };
            Shell.AddTabItem("SCRIPT", editor);
        }
    }
}