using OneCSharp.AST.UI;
using OneCSharp.Integrator.Model;
using OneCSharp.Integrator.Services;
using OneCSharp.Metadata.Services;
using OneCSharp.MVVM;
using OneCSharp.Scripting.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace OneCSharp.Integrator.Module
{
    public sealed class IntegratorModule : IModule
    {
        private const string MODULE_NAME = "Integrator";
        private const string SETTINGS_FILE_NAME = "IntegratorSettings.json";
        private const string CONTRACTS_CATALOG_NAME = "Contracts";
        private const string WEB_SERVERS_CATALOG_NAME = "WebServers";
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private readonly Dictionary<Type, object> _providers = new Dictionary<Type, object>();
        private readonly Dictionary<Type, IController> _controllers = new Dictionary<Type, IController>();
        public IntegratorModule() { }
        public T GetService<T>()
        {
            return (T)_services[typeof(T)];
        }
        public T GetProvider<T>()
        {
            return (T)_providers[typeof(T)];
        }
        public T GetController<T>()
        {
            return (T)_controllers[typeof(T)];
        }
        public IController GetController(Type type)
        {
            return _controllers[type];
        }
        public IShell Shell { get; private set; }
        public IntegratorModuleSettings Settings { get; private set; }
        public string ModuleCatalogPath
        {
            get
            {
                string moduleCatalog = Path.Combine(Shell.ModulesCatalogPath, MODULE_NAME);
                if (!Directory.Exists(moduleCatalog))
                {
                    _ = Directory.CreateDirectory(moduleCatalog);
                }
                return moduleCatalog;
            }
        }
        public string ContractsCatalogPath
        {
            get
            {
                string contractsCatalog = Path.Combine(ModuleCatalogPath, CONTRACTS_CATALOG_NAME);
                if (!Directory.Exists(contractsCatalog))
                {
                    _ = Directory.CreateDirectory(contractsCatalog);
                }
                return contractsCatalog;
            }
        }
        public string WebServersCatalogPath
        {
            get
            {
                string catalog = Path.Combine(ModuleCatalogPath, WEB_SERVERS_CATALOG_NAME);
                if (!Directory.Exists(catalog))
                {
                    _ = Directory.CreateDirectory(catalog);
                }
                return catalog;
            }
        }
        public void Initialize(IShell shell)
        {
            Shell = shell ?? throw new ArgumentNullException(nameof(shell));

            IModule module = Shell.GetModules().Where(m => m.GetType().Name == "MetadataModule").FirstOrDefault();
            if (module != null)
            {
                PropertyInfo property = module.GetType().GetProperty("Metadata");
                if (property != null)
                {
                    IMetadataService metadata = (IMetadataService)property.GetValue(module);
                    _services.Add(typeof(IMetadataService), metadata);
                    _services.Add(typeof(IScriptingService), new ScriptingService(metadata, new QueryExecutor(metadata)));
                }
            }
            LoadSettings();
            ConfigureProviders();
            ConfigureControllers();
            ConfigureView();
            ConfigureLanguage();
        }
        private void ConfigureProviders()
        {
            ContractsProvider provider = new ContractsProvider();
            foreach (string file in Directory.GetFiles(ContractsCatalogPath, "*.dll"))
            {
                Assembly contract = Assembly.LoadFrom(file);
                provider.AddContract(contract);
            }
            _providers.Add(typeof(ContractsProvider), provider);
        }
        private void ConfigureControllers()
        {
            _controllers.Add(typeof(ContractsController), new ContractsController(this));
            _controllers.Add(typeof(IntegratorModuleController), new IntegratorModuleController(this));
        }
        private void ConfigureView()
        {
            IController controller = GetController<IntegratorModuleController>();
            controller.BuildTreeNode(null, out TreeNodeViewModel mainNode);
            Shell.AddTreeNode(mainNode);
        }
        private void ConfigureLanguage()
        {
            SyntaxTreeController.Current.RegisterConceptLayout(
                typeof(CreateIntegrationNodeConcept),
                (IConceptLayout)Activator.CreateInstance(typeof(CreateIntegrationNodeLayout)));
        }

        private string SettingsFilePath
        {
            get { return Path.Combine(ModuleCatalogPath, SETTINGS_FILE_NAME); }
        }
        private void LoadSettings()
        {
            if (!File.Exists(SettingsFilePath))
            {
                IntegratorModuleSettings settings = new IntegratorModuleSettings();
                settings.Nodes.Add(new WebServerSettings() { Name = "оля" });
                SaveSettings(settings);
                Settings = settings;
            }
            else
            {
                byte[] json = File.ReadAllBytes(SettingsFilePath);
                Utf8JsonReader reader = new Utf8JsonReader(json);
                Settings = JsonSerializer.Deserialize<IntegratorModuleSettings>(ref reader);
            }
        }
        private void SaveSettings(IntegratorModuleSettings settings)
        {
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var options = new JsonSerializerOptions { WriteIndented = true };
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(settings, options);
            File.WriteAllBytes(SettingsFilePath, json);
            Settings = settings;
        }
        public void SaveSettings()
        {
            SaveSettings(Settings);
        }
    }
}