using Microsoft.VisualStudio.PlatformUI;
using OneCSharp.Metadata;
using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace OneCSharp.OQL.UI
{
    public sealed class ProcedureViewModel : SyntaxNodeViewModel, IOneCSharpCodeEditor
    {
        private Procedure _model;
        public event SaveSyntaxNodeEventHandler Save;
        public MetadataProvider Metadata { get; set; }

        public ProcedureViewModel()
        {
            _model = new Procedure();
            InitializeViewModel();
        }
        public ProcedureViewModel(Procedure model)
        {
            _model = model;
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            this.SaveProcedureCommand = new DelegateCommand(SaveProcedure);

            this.Parameters = new SyntaxNodeListViewModel();
            this.Statements = new SyntaxNodeListViewModel();

            if (_model.Parameters != null && _model.Parameters.Count > 0)
            {
                foreach (var parameter in _model.Parameters)
                {
                    this.Parameters.Add(new ParameterViewModel((Parameter)parameter));
                }
            }

            if (_model.Statements != null && _model.Statements.Count > 0)
            {
                foreach (var statement in _model.Statements)
                {
                    if (statement is SelectStatement)
                    {
                        this.Statements.Add(new SelectStatementViewModel((SelectStatement)statement));
                    }
                }
            }
        }
        public string Keyword { get { return _model.Keyword; } }
        public string Name
        {
            get { return string.IsNullOrEmpty(_model.Name) ? "<procedure name>" : _model.Name; }
            set { _model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public SyntaxNodeListViewModel Parameters { get; private set; } // ObservableCollection<SyntaxNodeViewModel>
        public SyntaxNodeListViewModel Statements { get; private set; }


        public bool IsModified { get; private set; } = true; // new procedure is unmodified by default
        public void EditSyntaxNode(ISyntaxNode node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (!(node is Procedure)) throw new ArgumentException(nameof(node));
            _model = (Procedure)node;
            IsModified = false;
        }
        public ICommand SaveProcedureCommand { get; private set; }
        public void SaveProcedure()
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
            _model.Parameters.Add(parameter);
            this.Parameters.Add(new ParameterViewModel(parameter) { Parent = this });
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
            _model.Statements.Add(select);
            this.Statements.Add(new SelectStatementViewModel(select) { Parent = this });
        }
    }
}
