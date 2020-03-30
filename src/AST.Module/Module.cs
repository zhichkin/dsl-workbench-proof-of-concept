using Microsoft.Win32;
using OneCSharp.AST.Model;
using OneCSharp.AST.Services;
using OneCSharp.AST.UI;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.Module
{
    public sealed class Module : IModule
    {
        #region " String resources "
        private const string ONE_C_SHARP = "ONE-C-SHARP";
        private const string MODULE_NAME = "DSL";
        private const string EDIT_WINDOW = "pack://application:,,,/OneCSharp.AST.Module;component/images/EditWindow.png";
        private const string OPEN_SCRIPT = "pack://application:,,,/OneCSharp.AST.Module;component/images/OpenFile.png";
        private const string SAVE_SCRIPT = "pack://application:,,,/OneCSharp.AST.Module;component/images/SaveToFile.png";
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
        public string ModuleCatalogPath
        {
            get { return Path.Combine(Shell.ModulesCatalogPath, MODULE_NAME); }
        }
        private string ScriptFilePath
        {
            get
            {
                string moduleCatalogPath = Path.Combine(Shell.ModulesCatalogPath, MODULE_NAME);
                if (!Directory.Exists(moduleCatalogPath))
                {
                    Directory.CreateDirectory(moduleCatalogPath);
                }
                return Path.Combine(ModuleCatalogPath, "script.json");
            }
        }
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
            Shell.AddMenuItem(new MenuItemViewModel()
            {
                MenuItemIcon = new BitmapImage(new Uri(OPEN_SCRIPT)),
                MenuItemHeader = "Open script file",
                MenuItemCommand = new RelayCommand(OpenScript),
                MenuItemPayload = this
            });
            Shell.AddMenuItem(new MenuItemViewModel()
            {
                MenuItemIcon = new BitmapImage(new Uri(SAVE_SCRIPT)),
                MenuItemHeader = "Save current script",
                MenuItemCommand = new RelayCommand(SaveScript),
                MenuItemPayload = this
            });
        }
        private void CreateCodeEditor(object parameter)
        {
            ScriptConcept script = new ScriptConcept();
            CodeEditor editor = new CodeEditor()
            {
                DataContext = SyntaxTreeController.Current.CreateSyntaxNode(null, script)
            };
            Shell.AddTabItem("SCRIPT", editor);
        }
        private void SaveScript(object parameter)
        {
            ConceptNodeViewModel concept = Shell.SelectedTabViewModel as ConceptNodeViewModel;
            if (concept == null)
            {
                MessageBox.Show("Script is not selected!", ONE_C_SHARP, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (!(concept.SyntaxNode is ScriptConcept script)) return;

            SyntaxTreeJsonSerializer _serializer = new SyntaxTreeJsonSerializer();
            var knownTypes = _serializer.Binder.KnownTypes;
            knownTypes.Add(typeof(ScriptConcept).FullName, typeof(ScriptConcept));
            knownTypes.Add(typeof(LanguageConcept).FullName, typeof(LanguageConcept));
            foreach (LanguageConcept language in script.Languages)
            {
                if (language.Assembly == null)
                {
                    continue;
                }
                foreach (Type type in language.Assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(SyntaxNode))))
                {
                    knownTypes.Add(type.FullName, type);
                }
            }
            if (knownTypes.Count == 0) return;

            string filePath = ScriptFilePath;
            string json = _serializer.ToJson(script);
            using (StreamWriter writer = File.CreateText(filePath))
            {
                writer.Write(json);
            }

            MessageBox.Show("Script has been saved successfully!", ONE_C_SHARP, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void OpenScript(object parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Multiselect = false,
                InitialDirectory = ModuleCatalogPath,
                Filter = "Script files (.json)|*.json"
            };
            var result = dialog.ShowDialog();
            if (result != true) return;
            string filePath = dialog.FileName;

            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json)) return;

            SyntaxTreeJsonSerializer _serializer = new SyntaxTreeJsonSerializer();
            var knownTypes = _serializer.Binder.KnownTypes;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith("OneCSharp") && a.GetName().Name.EndsWith("Model")))
            {
                foreach (Type type in assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(SyntaxNode))))
                {
                    knownTypes.Add(type.FullName, type);
                }
            }
            if (knownTypes.Count == 0) return;

            SyntaxNode syntaxNode = _serializer.FromJson(json);

            CodeEditor editor = new CodeEditor()
            {
                DataContext = SyntaxTreeController.Current.CreateSyntaxNode(null, syntaxNode)
            };
            Shell.AddTabItem("SCRIPT FROM FILE", editor);
        }
    }
}