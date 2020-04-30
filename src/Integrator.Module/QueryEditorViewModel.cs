using Microsoft.SqlServer.TransactSql.ScriptDom;
using OneCSharp.Integrator.Model;
using OneCSharp.Integrator.Services;
using OneCSharp.Metadata.Services;
using OneCSharp.MVVM;
using OneCSharp.Scripting.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF = System.Windows.Controls;

namespace OneCSharp.Integrator.Module
{
    public sealed class QueryEditorViewModel : ViewModelBase
    {
        private string _queryScript = string.Empty;
        public QueryEditorViewModel(IShell shell, WebServerSettings settigns, string fileFullPath, IMetadataService metadata, IScriptingService scripting)
        {
            if (shell == null) throw new ArgumentNullException(nameof(shell));
            if (settigns == null) throw new ArgumentNullException(nameof(settigns));
            if (scripting == null) throw new ArgumentNullException(nameof(scripting));
            if (string.IsNullOrWhiteSpace(fileFullPath)) throw new ArgumentNullException(nameof(fileFullPath));

            Shell = shell;
            Settings = settigns;
            Metadata = metadata;
            Scripting = scripting;
            FileFullPath = fileFullPath;

            ExecuteCommand = new RelayCommand(Execute);
            TranslateCommand = new RelayCommand(Translate);
        }
        private IShell Shell { get; }
        public string FileFullPath { get; }
        public WebServerSettings Settings { get; }
        private IMetadataService Metadata { get; }
        private IScriptingService Scripting { get; }
        public string QueryScript
        {
            get { return _queryScript; }
            set { _queryScript = value; OnPropertyChanged(nameof(QueryScript)); }
        }
        public ICommand ExecuteCommand { get; private set; }
        public ICommand TranslateCommand { get; private set; }
        private void Translate(object parameter)
        {
            Metadata.UseServer(Settings.DataHost);
            Metadata.UseDatabase(Settings.Database);
            string sql = Scripting.PrepareScript(QueryScript, out IList<ParseError> errors);
            string errorMessage = string.Empty;
            foreach (ParseError error in errors)
            {
                errorMessage += error.Message + Environment.NewLine;
            }

            if (errors.Count > 0)
            {
                QueryEditorView view = new QueryEditorView()
                {
                    DataContext = new QueryEditorViewModel(Shell, Settings, FileFullPath, Metadata, Scripting) { QueryScript = errorMessage }
                };
                Shell.AddTabItem("Errors", view);
            }
            else
            {
                FileInfo file = new FileInfo(FileFullPath);
                string fileFullPath = Path.ChangeExtension(FileFullPath, "sql");
                QueryEditorView view = new QueryEditorView()
                {
                    DataContext = new QueryEditorViewModel(Shell, Settings, fileFullPath, Metadata, Scripting) { QueryScript = sql }
                };
                Shell.AddTabItem(Path.ChangeExtension(file.Name, "sql"), view);
            }
        }
        private void Execute(object parameter)
        {
            string errorMessage = string.Empty;

            Metadata.UseServer(Settings.DataHost);
            Metadata.UseDatabase(Settings.Database);

            string sql;
            FileInfo file = new FileInfo(FileFullPath);
            if (file.Extension == ".sql")
            {
                sql = QueryScript;
            }
            else
            {
                sql = Scripting.PrepareScript(QueryScript, out IList<ParseError> errors);
                foreach (ParseError error in errors)
                {
                    errorMessage += error.Message + Environment.NewLine;
                }
                if (errors.Count > 0)
                {
                    QueryEditorView errorsView = new QueryEditorView()
                    {
                        DataContext = new QueryEditorViewModel(Shell, Settings, FileFullPath, Metadata, Scripting) { QueryScript = errorMessage }
                    };
                    Shell.AddTabItem("Errors", errorsView);
                    return;
                }
            }
            string json = "[]";
            try
            {
                json = Scripting.ExecuteScript(sql, out IList<ParseError> executeErrors);
                foreach (ParseError error in executeErrors)
                {
                    errorMessage += error.Message + Environment.NewLine;
                }
                if (executeErrors.Count > 0)
                {
                    QueryEditorView errorsView = new QueryEditorView()
                    {
                        DataContext = new QueryEditorViewModel(Shell, Settings, FileFullPath, Metadata, Scripting) { QueryScript = errorMessage }
                    };
                    Shell.AddTabItem("Errors", errorsView);
                    return;
                }
            }
            catch (Exception ex)
            {
                QueryEditorView errorsView = new QueryEditorView()
                {
                    DataContext = new QueryEditorViewModel(Shell, Settings, FileFullPath, Metadata, Scripting) { QueryScript = ex.Message }
                };
                Shell.AddTabItem("Error", errorsView);
                return;
            }
            JsonSerializerOptions serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new DynamicJsonConverter());
            dynamic data = JsonSerializer.Deserialize<dynamic>(json, serializerOptions);

            //QueryEditorView view = new QueryEditorView()
            //{
            //    DataContext = new QueryEditorViewModel(Shell, Settings, FileFullPath, Metadata, Scripting) { QueryScript = json }
            //};
            //Grid view = CreateDynamicGrid(data);
            DataGrid view = CreateDynamicDataGrid(data);
            Shell.AddTabItem($"{file.Name} - result", view);
        }
        private Grid CreateDynamicGrid(dynamic data)
        {
            Grid grid = new Grid()
            {
                ShowGridLines = true,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            if (data is IList list)
            {
                RowDefinition rowDef;
                rowDef = new RowDefinition()
                {
                    Height = new GridLength()
                };
                grid.RowDefinitions.Add(rowDef); // headers row

                for (int i = 0; i < list.Count; i++)
                {
                    rowDef = new RowDefinition()
                    {
                        Height = new GridLength()
                    };
                    grid.RowDefinitions.Add(rowDef);
                }

                if (list.Count > 0)
                {
                    ExpandoObject item = list[0] as ExpandoObject;
                    int ii = 0;
                    foreach (var column in item)
                    {
                        WPF.ColumnDefinition colDef = new WPF.ColumnDefinition()
                        {
                            Width = new GridLength(1, GridUnitType.Auto)
                        };
                        grid.ColumnDefinitions.Add(colDef);

                        TextBlock block = new TextBlock()
                        {
                            Text = column.Key,
                            FontSize = 14,
                            FontWeight = FontWeights.Bold,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        Grid.SetRow(block, 0);
                        Grid.SetColumn(block, ii);
                        grid.Children.Add(block);
                        ii++;
                    }
                }
                int r = 0;
                int c = 0;
                foreach (ExpandoObject obj in list)
                {
                    r++;
                    foreach (var item in obj)
                    {
                        TextBlock block = new TextBlock()
                        {
                            Text = item.Value == null ? string.Empty : item.Value.ToString(),
                            FontSize = 14,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        Grid.SetRow(block, r);
                        Grid.SetColumn(block, c);
                        grid.Children.Add(block);
                        c++;
                    }
                }
            }
            
            return grid;
        }
        private DataGrid CreateDynamicDataGrid(dynamic data)
        {
            List<Dictionary<string, object>> source = new List<Dictionary<string, object>>();
            if (data is IEnumerable list)
            {
                foreach (ExpandoObject item in list)
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    foreach (var value in item)
                    {
                        row.Add(value.Key.Replace('-', '_'), value.Value);
                    }
                    source.Add(row);
                }
            }
            DataGrid grid = new DataGrid()
            {
                ItemsSource = source.ToDataSource(),
                AutoGenerateColumns = true,
                CanUserResizeColumns = true
            };
            return grid;
        }
    }
}