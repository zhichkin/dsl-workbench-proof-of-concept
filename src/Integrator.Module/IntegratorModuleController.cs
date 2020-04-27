using OneCSharp.Integrator.Model;
using OneCSharp.MVVM;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OneCSharp.Integrator.Module
{
    public sealed class IntegratorModuleController : IController
    {
        #region "Private fields"
        private const string MODULE_NAME = "Integrator";
        private const string MODULE_TOOLTIP = "Integrator module";
        private const string ROOT_NODE_NAME = "WEB";
        private const string ROOT_NODE_TOOLTIP = "Web servers";
        private const string SETTINGS_FILE_NAME = "WebServerSettings.json";
        private const string QUERY_FILE_SEARCH_PATTERN = "*.qry";
        private const string ADD_CATALOG_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/AddCatalog.png";
        private const string SETTINGS_FILE_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/SettingsFile.png";

        private const string MODULE_ICON_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/Cloud.png";
        private const string WEB_SERVER_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/WebServer.png";
        private const string WEB_SERVICE_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/WebService.png";
        private const string WEB_CATALOG_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/WebCatalog.png";
        private const string DATABASE_SCRIPT_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/DatabaseScript.png";

        private const string SERVER_SETTINGS_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/ServerSettings.png";
        private const string ADD_WEB_SERVICE_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/AddWebService.png";
        private const string SAVE_FILE_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/SaveToFile.png";
        private const string CANCEL_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/Cancel.png";
        private const string ADD_SERVER_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/AddLocalServer.png";
        private const string DATABASE_SERVER_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/DataServer.png";
        private const string ADD_DATABASE_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/AddDatabase.png";
        private const string DATABASE_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/Database.png";
        private const string UPDATE_DATABASE_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/UpdateDatabase.png";
        private const string CATALOG_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/Catalog.png";

        private readonly BitmapImage MODULE_ICON = new BitmapImage(new Uri(MODULE_ICON_PATH));
        private readonly BitmapImage ADD_CATALOG_ICON = new BitmapImage(new Uri(ADD_CATALOG_PATH));
        private readonly BitmapImage SETTINGS_FILE_ICON = new BitmapImage(new Uri(SETTINGS_FILE_PATH));

        private readonly BitmapImage WEB_SERVER_ICON = new BitmapImage(new Uri(WEB_SERVER_PATH));
        private readonly BitmapImage WEB_SERVICE_ICON = new BitmapImage(new Uri(WEB_SERVICE_PATH));
        private readonly BitmapImage WEB_CATALOG_ICON = new BitmapImage(new Uri(WEB_CATALOG_PATH));
        private readonly BitmapImage ADD_WEB_SERVICE_ICON = new BitmapImage(new Uri(ADD_WEB_SERVICE_PATH));
        private readonly BitmapImage DATABASE_SCRIPT_ICON = new BitmapImage(new Uri(DATABASE_SCRIPT_PATH));

        private readonly BitmapImage SAVE_FILE_ICON = new BitmapImage(new Uri(SAVE_FILE_PATH));
        private readonly BitmapImage SERVER_SETTINGS_ICON = new BitmapImage(new Uri(SERVER_SETTINGS_PATH));
        private readonly BitmapImage CANCEL_ICON = new BitmapImage(new Uri(CANCEL_PATH));
        private readonly BitmapImage ADD_SERVER_ICON = new BitmapImage(new Uri(ADD_SERVER_PATH));
        private readonly BitmapImage DATABASE_SERVER_ICON = new BitmapImage(new Uri(DATABASE_SERVER_PATH));
        private readonly BitmapImage ADD_DATABASE_ICON = new BitmapImage(new Uri(ADD_DATABASE_PATH));
        private readonly BitmapImage DATABASE_ICON = new BitmapImage(new Uri(DATABASE_PATH));
        private readonly BitmapImage UPDATE_DATABASE_ICON = new BitmapImage(new Uri(UPDATE_DATABASE_PATH));
        private readonly BitmapImage CATALOG_ICON = new BitmapImage(new Uri(CATALOG_PATH));
        #endregion
        private IntegratorModule Module { get; set; }
        private TreeNodeViewModel RootNode { get; set; }
        public IntegratorModuleController(IModule module)
        {
            Module = (IntegratorModule)module;
        }
        
        #region " Utility methods "
        private TreeNodeViewModel FindTreeNode(WebServerSettings settings)
        {
            foreach (TreeNodeViewModel treeNode in RootNode.TreeNodes)
            {
                if (treeNode.NodePayload == settings) return treeNode;
            }
            return null;
        }
        private byte[] SerializeWebServerSettings(WebServerSettings settings)
        {
            // do not encode any characters for web transfer
            JavaScriptEncoder encoder = JavaScriptEncoder.Create(new UnicodeRange(0, 0xFFFF)); // 65535
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Encoder = encoder,
                WriteIndented = true
            };
            return JsonSerializer.SerializeToUtf8Bytes(settings, options);
        }
        #endregion

        public void AttachTreeNodes(TreeNodeViewModel parentNode)
        {
            throw new NotImplementedException();
        }
        public void BuildTreeNode(object model, out TreeNodeViewModel treeNode)
        {
            // Main Shell menu commands
            Module.Shell.AddMenuItem(new MenuItemViewModel()
            {
                MenuItemIcon = SAVE_FILE_ICON,
                MenuItemHeader = "Save current file",
                MenuItemCommand = new RelayCommand(SaveScript),
                MenuItemPayload = Module
            });

            treeNode = new TreeNodeViewModel()
            {
                IsExpanded = true,
                NodeIcon = WEB_CATALOG_ICON,
                NodeText = ROOT_NODE_NAME,
                NodeToolTip = ROOT_NODE_TOOLTIP,
                NodePayload = null
            };
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Attach web server",
                MenuItemIcon = ADD_WEB_SERVICE_ICON,
                MenuItemCommand = new RelayCommand(AttachWebServer),
                MenuItemPayload = treeNode
            });
            treeNode.ContextMenuItems.Add(new MenuItemViewModel() { IsSeparator = true });
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "About...",
                MenuItemIcon = MODULE_ICON,
                MenuItemCommand = new RelayCommand(ShowAboutWindow),
                MenuItemPayload = treeNode
            });
            RootNode = treeNode;

            DirectoryInfo catalog = new DirectoryInfo(Module.WebServersCatalogPath);
            if (catalog.Exists)
            {
                LoadCatalogStructure(catalog, treeNode, true);
            }
        }
        private void ShowAboutWindow(object parameter)
        {
            MessageBox.Show("1C# Integrator module © 2020"
                + Environment.NewLine
                + Environment.NewLine + "Created by Zhichkin"
                + Environment.NewLine + "dima_zhichkin@mail.ru"
                + Environment.NewLine + "https://github.com/zhichkin/",
                "1C#",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        
        private void LoadCatalogStructure(DirectoryInfo catalog, TreeNodeViewModel parentNode, bool isServerLevel)
        {
            if (!catalog.Exists) return;

            TreeNodeViewModel catalogNode = null;
            DirectoryInfo[] catalogs = catalog.GetDirectories();
            foreach (DirectoryInfo directory in catalogs)
            {
                if (isServerLevel)
                {
                    WebServerSettings settings;
                    string settingsFilePath = Path.Combine(directory.FullName, SETTINGS_FILE_NAME);
                    if (File.Exists(settingsFilePath))
                    {
                        settings = LoadWebServerSettings(settingsFilePath);
                    }
                    else
                    {
                        settings = new WebServerSettings() { Name = directory.Name };
                        SaveWebServerSettings(settings, settingsFilePath);
                    }
                    catalogNode = CreateWebServerNode(parentNode, settings);
                }
                else
                {
                    catalogNode = CreateCatalogNode(parentNode, directory.Name);
                }

                LoadCatalogStructure(directory, catalogNode, false);

                FileInfo[] files = directory.GetFiles(QUERY_FILE_SEARCH_PATTERN);
                foreach (FileInfo file in files)
                {
                    CreateScriptNode(catalogNode, file.Name);
                }
            }
        }
        private WebServerSettings LoadWebServerSettings(string settingsFilePath)
        {
            byte[] json = File.ReadAllBytes(settingsFilePath);
            Utf8JsonReader reader = new Utf8JsonReader(json);
            WebServerSettings settings = JsonSerializer.Deserialize<WebServerSettings>(ref reader);
            return settings;
        }
        private TreeNodeViewModel CreateWebServerNode(TreeNodeViewModel parentNode, WebServerSettings settings)
        {
            TreeNodeViewModel treeNode = new TreeNodeViewModel()
            {
                IsExpanded = false,
                NodeIcon = WEB_SERVICE_ICON,
                NodeText = settings.Name,
                NodeToolTip = settings.HttpHost,
                NodePayload = settings
            };
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Configure settings",
                MenuItemIcon = SETTINGS_FILE_ICON,
                MenuItemCommand = new RelayCommand(ConfigureWebServer),
                MenuItemPayload = treeNode
            });
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Create new catalog",
                MenuItemIcon = ADD_CATALOG_ICON,
                MenuItemCommand = new RelayCommand(CreateWebCatalog),
                MenuItemPayload = treeNode
            });
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Create new script",
                MenuItemIcon = DATABASE_SCRIPT_ICON,
                MenuItemCommand = new RelayCommand(CreateWebScript),
                MenuItemPayload = treeNode
            });
            parentNode.TreeNodes.Add(treeNode);

            return treeNode;
        }
        private TreeNodeViewModel CreateCatalogNode(TreeNodeViewModel parentNode, string catalogName)
        {
            TreeNodeViewModel treeNode = new TreeNodeViewModel()
            {
                IsExpanded = false,
                NodeIcon = CATALOG_ICON,
                NodeText = catalogName,
                NodeToolTip = catalogName
            };
            if (parentNode.NodePayload is WebServerSettings)
            {
                treeNode.NodePayload = Path.Combine(parentNode.NodeText, catalogName);
            }
            else
            {
                treeNode.NodePayload = Path.Combine((string)parentNode.NodePayload, catalogName);
            }
            // TODO: edit catalog name menu option
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Create new catalog",
                MenuItemIcon = ADD_CATALOG_ICON,
                MenuItemCommand = new RelayCommand(CreateWebCatalog),
                MenuItemPayload = treeNode
            });
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Create new script",
                MenuItemIcon = DATABASE_SCRIPT_ICON,
                MenuItemCommand = new RelayCommand(CreateWebScript),
                MenuItemPayload = treeNode
            });
            parentNode.TreeNodes.Add(treeNode);

            return treeNode;
        }
        private void CreateScriptNode(TreeNodeViewModel parentNode, string fileName)
        {
            TreeNodeViewModel treeNode = new TreeNodeViewModel()
            {
                NodeIcon = DATABASE_SCRIPT_ICON,
                NodeText = fileName,
                NodeToolTip = fileName,
                NodePayload = null
            };
            if (parentNode.NodePayload is WebServerSettings)
            {
                treeNode.NodePayload = parentNode.NodeText;
            }
            else
            {
                treeNode.NodePayload = (string)parentNode.NodePayload;
            }
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Edit",
                MenuItemIcon = DATABASE_SCRIPT_ICON,
                MenuItemCommand = new RelayCommand(EditWebScript),
                MenuItemPayload = treeNode
            });
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Deploy",
                MenuItemIcon = ADD_CATALOG_ICON,
                MenuItemCommand = null,
                MenuItemPayload = treeNode
            });
            parentNode.TreeNodes.Add(treeNode);
        }

        #region " Main menu commands "
        private void SaveScript(object parameter)
        {
            if (Module.Shell.SelectedTabViewModel is WebServerViewModel vm)
            {
                SaveWebServerScript(vm.Model, vm.TextJSON);
            }
            else if (Module.Shell.SelectedTabViewModel is QueryEditorViewModel editor)
            {
                SaveQueryScript(editor.FileFullPath, editor.QueryScript);
            }
        }
        #endregion

        private void AttachWebServer(object parameter)
        {
            if (!(parameter is TreeNodeViewModel parentNode)) return;

            InputStringDialog dialog = new InputStringDialog()
            {
                Title = "Web server name"
            };
            _ = dialog.ShowDialog();
            if (dialog.Result == null) return;

            string catalogName = Path.Combine(Module.WebServersCatalogPath, (string)dialog.Result);
            DirectoryInfo catalog = new DirectoryInfo(catalogName);
            if (catalog.Exists)
            {
                MessageBox.Show($"Catalog \"{catalogName}\" already exists !", "1C#", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            catalog.Create();

            WebServerSettings settings = new WebServerSettings()
            {
                Name = catalog.Name
            };
            string settingsFilePath = Path.Combine(catalog.FullName, SETTINGS_FILE_NAME);
            SaveWebServerSettings(settings, settingsFilePath);

            TreeNodeViewModel treeNode = CreateWebServerNode(parentNode, settings);
            treeNode.IsExpanded = true;
        }
        private void ConfigureWebServer(object parameter)
        {
            if (!(parameter is TreeNodeViewModel treeNode)) return;
            if (!(treeNode.NodePayload is WebServerSettings settings)) return;

            byte[] bytes = SerializeWebServerSettings(settings);
            string json = Encoding.UTF8.GetString(bytes);

            WebServerView view = new WebServerView()
            {
                DataContext = new WebServerViewModel(settings) { TextJSON = json }
            };
            Module.Shell.AddTabItem(settings.Name, view);
        }
        private void SaveWebServerSettings(WebServerSettings settings, string settingsFilePath)
        {
            byte[] json = SerializeWebServerSettings(settings);
            File.WriteAllBytes(settingsFilePath, json);
        }
        private void SaveWebServerScript(WebServerSettings settings, string json)
        {
            string catalogPath = Path.Combine(Module.WebServersCatalogPath, settings.Name);
            DirectoryInfo catalog = new DirectoryInfo(catalogPath);
            if (!catalog.Exists)
            {
                MessageBox.Show($"Catalog \"{catalogPath}\" is not found !", "1C#", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string settingsFilePath = Path.Combine(catalog.FullName, SETTINGS_FILE_NAME);
            if (!File.Exists(settingsFilePath))
            {
                MessageBox.Show($"File \"{settingsFilePath}\" is not found !", "1C#", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            WebServerSettings newSettings = JsonSerializer.Deserialize<WebServerSettings>(json);
            string newCatalogPath = Path.Combine(Module.WebServersCatalogPath, newSettings.Name);
            if (Directory.Exists(newCatalogPath))
            {
                MessageBox.Show($"Catalog \"{newCatalogPath}\" already exists !", "1C#", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            settings.Name = newSettings.Name;
            settings.HttpHost = newSettings.HttpHost;
            SaveWebServerSettings(settings, settingsFilePath);
            Directory.Move(catalogPath, newCatalogPath);

            TreeNodeViewModel treeNode = FindTreeNode(settings);
            if (treeNode != null)
            {
                treeNode.NodeText = settings.Name;
                treeNode.NodeToolTip = settings.HttpHost;
            }
        }

        private void CreateWebCatalog(object parameter)
        {
            if (!(parameter is TreeNodeViewModel parentNode)) return;

            // ask for catalog name
            InputStringDialog dialog = new InputStringDialog()
            {
                Title = "Catalog name"
            };
            _ = dialog.ShowDialog();
            if (dialog.Result == null) return;

            string catalogName = (string)dialog.Result;
            string catalogPath;
            if (parentNode.NodePayload is WebServerSettings)
            {
                catalogPath = Path.Combine(parentNode.NodeText, catalogName);
            }
            else
            {
                catalogPath = Path.Combine((string)parentNode.NodePayload, catalogName);
            }
            string catalogFullName = Path.Combine(Module.WebServersCatalogPath, catalogPath);
            if (Directory.Exists(catalogFullName))
            {
                MessageBox.Show($"Catalog \"{catalogPath}\" already exists !", "1C#", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            // create direcitory on the disk
            _ = Directory.CreateDirectory(catalogFullName);

            TreeNodeViewModel treeNode = CreateCatalogNode(parentNode, catalogName);
            treeNode.IsExpanded = true;
            parentNode.IsExpanded = true;
        }

        private void CreateWebScript(object parameter)
        {
            if (!(parameter is TreeNodeViewModel parentNode)) return;

            // ask for query file name
            InputStringDialog dialog = new InputStringDialog()
            {
                Title = "Script name"
            };
            _ = dialog.ShowDialog();
            if (dialog.Result == null) return;

            string scriptName = (string)dialog.Result;
            string catalogPath;
            if (parentNode.NodePayload is WebServerSettings)
            {
                catalogPath = parentNode.NodeText;
            }
            else
            {
                catalogPath = (string)parentNode.NodePayload;
            }
            string scriptFullName = Path.Combine(Module.WebServersCatalogPath, catalogPath, $"{scriptName}.qry");
            if (File.Exists(scriptFullName))
            {
                MessageBox.Show($"Script \"{scriptFullName}\" already exists !", "1C#", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            // create query file on the disk
            _ = File.Create(scriptFullName);

            CreateScriptNode(parentNode, $"{scriptName}.qry");

            // open script for editing
            QueryEditorView view = new QueryEditorView()
            {
                DataContext = new QueryEditorViewModel(scriptFullName) { QueryScript = "" }
            };
            Module.Shell.AddTabItem($"{scriptName}.qry", view);
        }
        private void EditWebScript(object parameter)
        {
            if (!(parameter is TreeNodeViewModel parentNode)) return;

            string scriptName = parentNode.NodeText;
            string catalogPath = (string)parentNode.NodePayload;
            string scriptFullName = Path.Combine(Module.WebServersCatalogPath, catalogPath, scriptName);
            if (!File.Exists(scriptFullName))
            {
                MessageBox.Show($"Script \"{scriptFullName}\" not found !", "1C#", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            // read script file
            string script = File.ReadAllText(scriptFullName);

            // open script for editing
            QueryEditorView view = new QueryEditorView()
            {
                DataContext = new QueryEditorViewModel(scriptFullName) { QueryScript = script }
            };
            Module.Shell.AddTabItem(scriptName, view);
        }
        private void SaveQueryScript(string filePath, string script)
        {
            File.WriteAllText(filePath, script, Encoding.UTF8);
        }
    }
}