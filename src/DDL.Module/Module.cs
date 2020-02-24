using OneCSharp.AST.Model;
using OneCSharp.AST.UI;
using OneCSharp.DDL.Model;
using OneCSharp.MVVM;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OneCSharp.DDL.Module
{
    public sealed class Module : IModule
    {
        #region " String resources "
        public const string ONE_C_SHARP = "ONE-C-SHARP";
        public const string EDIT_WINDOW = "pack://application:,,,/OneCSharp.AST.Module;component/images/EditWindow.png";
        #endregion

        public Module() { }
        public IController GetController<T>()
        {
            throw new NotImplementedException();
        }
        public IController GetController(Type type)
        {
            throw new NotImplementedException();
        }
        public IShell Shell { get; private set; }
        public void Initialize(IShell shell)
        {
            Shell = shell ?? throw new ArgumentNullException(nameof(shell));

            try
            {
                // TODO: move to the base abstract module class !
                RegisterConceptLayouts();
            }
            catch (Exception ex)
            {
                // TODO: log error !!!
                MessageBox.Show("Failed to load DDL module:"
                    + Environment.NewLine
                    + Environment.NewLine
                    + ex.Message,
                    ONE_C_SHARP, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            Shell.AddMenuItem(new MenuItemViewModel()
            {
                MenuItemIcon = new BitmapImage(new Uri(EDIT_WINDOW)),
                MenuItemHeader = "Add test DDL script",
                MenuItemCommand = new RelayCommand(CreateTestCodeEditor),
                MenuItemPayload = this
            });
        }
        private void RegisterConceptLayouts()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string modulePath = Path.GetDirectoryName(assembly.Location);
            string moduleName = Path.GetFileName(assembly.Location);
            string layoutsPath = moduleName.Replace("Module", "UI");
            string conceptsPath = moduleName.Replace("Module", "Model");
            layoutsPath = Path.Combine(modulePath, layoutsPath);
            conceptsPath = Path.Combine(modulePath, conceptsPath);
            // if (File.Exists(layoutsPath) && File.Exists(conceptsPath)) - not working sometimes ...
            //Assembly layouts;
            //Assembly concepts;
            //try
            //{
            //    // TODO: load assembly from binary image so as not to block the dll file !!!
            //    FileStream stream = File.OpenRead(layoutsPath);
            //    byte[] buffer = new byte[stream.Length];
            //    stream.Read(buffer, 0, (int)stream.Length);
            //    layouts = Assembly.Load(buffer);
            //    concepts = Assembly.LoadFrom(conceptsPath);
            //}
            //catch (Exception ex)
            //{
            //    //TODO: log error
            //    return;
            //}
            if (File.Exists(layoutsPath) && File.Exists(conceptsPath))
            {
                Assembly layouts = Assembly.LoadFrom(layoutsPath);
                Assembly concepts = Assembly.LoadFrom(conceptsPath);
                foreach (Type concept in concepts.GetTypes().Where(t => t.IsSubclassOf(typeof(SyntaxNode))))
                {
                    foreach (Type layout in layouts.GetTypes()
                        .Where(t => t.GetInterfaces()
                                    .Where(i => i == typeof(IConceptLayout))
                                    .Count() > 0))
                    {
                        if (layout.BaseType.IsGenericType
                            && layout.BaseType.GetGenericArguments()
                                .Where(t => t == concept).Count() > 0)
                        {
                            SyntaxTreeController.Current.RegisterConceptLayout(
                                concept,
                                (IConceptLayout)Activator.CreateInstance(layout));
                            break;
                        }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("DDL module files not found!");
            }
        }
        public void Persist(object entity)
        {
            throw new NotImplementedException();
        }
        private void CreateTestCodeEditor(object parameter)
        {
            DomainConcept concept = new DomainConcept();
            CodeEditor editor = new CodeEditor()
            {
                DataContext = SyntaxTreeController.Current.CreateSyntaxNode(null, concept)
            };
            Shell.AddTabItem("DDL SCRIPT", editor);
        }
    }
}