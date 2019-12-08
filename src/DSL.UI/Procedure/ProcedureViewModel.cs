using OneCSharp.DSL.Model;
using OneCSharp.DSL.Services;
using OneCSharp.DSL.UI.Services;
using OneCSharp.Metadata.Services;
using OneCSharp.MVVM;
using System.Windows.Input;

namespace OneCSharp.DSL.UI
{
    public sealed class ProcedureViewModel : SyntaxNodeViewModel, IOneCSharpCodeEditor
    {
        private readonly IShell _shell;
        public IMetadataProvider Metadata { get; set; }
        public event SaveSyntaxNodeEventHandler Save;

        public ProcedureViewModel()
        {
            _model = new Procedure();
            InitializeViewModel();
        }
        public ProcedureViewModel(IShell shell, Procedure model)
        {
            _shell = shell;
            _model = model;
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            RunProcedureCommand = new RelayCommand(RunProcedure);
            SaveProcedureCommand = new RelayCommand(SaveProcedure);

            Procedure model = Model as Procedure;

            this.Parameters = new SyntaxNodeListViewModel(this, model.Parameters);
            this.Statements = new SyntaxNodeListViewModel(this, model.Statements);

            if (model.Parameters != null && model.Parameters.Count > 0)
            {
                foreach (var parameter in model.Parameters)
                {
                    ParameterViewModel child = new ParameterViewModel(this, (Parameter)parameter);
                    this.Parameters.Add(child);
                }
            }

            if (model.Statements != null && model.Statements.Count > 0)
            {
                foreach (var statement in model.Statements)
                {
                    if (statement is SelectStatement)
                    {
                        SelectStatementViewModel child = new SelectStatementViewModel(this, (SelectStatement)statement);
                        this.Statements.Add(child);
                    }
                }
            }
        }
        public string Keyword { get { return ((Procedure)Model).Keyword; } }
        public string Name
        {
            get { return string.IsNullOrEmpty(((Procedure)Model).Name) ? "<procedure name>" : ((Procedure)Model).Name; }
            set { ((Procedure)Model).Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public SyntaxNodeListViewModel Parameters { get; private set; }
        public SyntaxNodeListViewModel Statements { get; private set; }


        public bool IsModified { get; private set; } = true; // new procedure is unmodified by default
        public ICommand RunProcedureCommand { get; private set; }
        public ICommand SaveProcedureCommand { get; private set; }

        public void SaveProcedure(object parameter)
        {
            CodeEditorEventArgs args = new CodeEditorEventArgs(_model);
            Save?.Invoke(this, args);
            if (args.Cancel == true)
            {
                // Save command was canceled by user
                // Take some action ...
            }
            else
            {
                IsModified = false;
            }
        }


        public void AddParameter()
        {
            Parameter parameter = new Parameter(_model);
            ((Procedure)Model).Parameters.Add(parameter);
            this.Parameters.Add(new ParameterViewModel(this, parameter));
        }
        public void RemoveParameter(ParameterViewModel parameter)
        {
            parameter.Parent = null;
            this.Parameters.Remove(parameter);
        }
        public void MoveParameterUp(ParameterViewModel parameter)
        {
            int index = this.Parameters.IndexOf(parameter);
            if (index == 0) return;

            this.Parameters.Remove(parameter);
            this.Parameters.Insert(--index, parameter);

            if (this.Parameters.Count > 1)
            {
                parameter.IsRemoveButtonVisible = false;
            }
        }
        public void MoveParameterDown(ParameterViewModel parameter)
        {
            int index = this.Parameters.IndexOf(parameter);
            if (index == (this.Parameters.Count - 1)) return;

            this.Parameters.Remove(parameter);
            this.Parameters.Insert(++index, parameter);

            if (this.Parameters.Count > 1)
            {
                parameter.IsRemoveButtonVisible = false;
            }
        }



        public void AddSelectStatement()
        {
            SelectStatement select = new SelectStatement(_model);
            ((Procedure)Model).Statements.Add(select);
            this.Statements.Add(new SelectStatementViewModel(this, select));
        }


        public void RunProcedure(object parameter)
        {
            if (_shell == null) return;
            Procedure model = (Procedure)Model;
            string sql = new SQLServerQueryBuilder().Build(model);
            _shell.AddTabItem($"{model.Name}.sql", UIServices.CreateTextBox(sql));
        }

        public void AddInsertStatement()
        {
            Procedure model = (Procedure)Model;
            InsertStatement insert = new InsertStatement(model);
            model.Statements.Add(insert);
            this.Statements.Add(UIServices.CreateViewModel(this, insert));
        }
    }
}
