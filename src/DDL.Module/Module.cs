using OneCSharp.AST.Model;
using OneCSharp.AST.UI;
using OneCSharp.DDL.Model;
using OneCSharp.MVVM;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace OneCSharp.DDL.Module
{
    public sealed class Module : IModule
    {
        #region " String resources "
        private const string ONE_C_SHARP = "ONE-C-SHARP";
        private const string DATABASES_CATALOG = "Databases";
        private const string EDIT_WINDOW = "pack://application:,,,/OneCSharp.AST.Module;component/images/EditWindow.png";
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
        public string DatabasesCatalog
        {
            get
            {
                string catalogPath = Path.Combine(Shell.AppCatalogPath, DATABASES_CATALOG);
                if (!Directory.Exists(catalogPath))
                {
                    _ = Directory.CreateDirectory(catalogPath);
                }
                return catalogPath;
            }
        }
        public void Initialize(IShell shell)
        {
            Shell = shell ?? throw new ArgumentNullException(nameof(shell));

            RegisterDatabaseAssemblies();

            SyntaxTreeManager.RegisterScopeProvider(typeof(EntityConcept), new EntityScopeProvider());

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
        }
        private void RegisterDatabaseAssemblies()
        {
            DatabaseScopeProvider databaseProvider = new DatabaseScopeProvider();
            SyntaxTreeManager.RegisterScopeProvider(typeof(UseDatabaseConcept), databaseProvider);

            //foreach (string directory in Directory.GetDirectories())
            foreach (string file in Directory.GetFiles(DatabasesCatalog, "*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(file);
                databaseProvider.RegisterDatabase(assembly);
            }
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