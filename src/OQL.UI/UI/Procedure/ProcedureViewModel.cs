﻿using OneCSharp.OQL.Model;
using System.Collections.ObjectModel;

namespace OneCSharp.OQL.UI
{
    public sealed class ProcedureViewModel : SyntaxNodeViewModel
    {
        private readonly Procedure _model;
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
        private void InitializeViewModel()
        {
            this.Parameters = new ObservableCollection<SyntaxNodeViewModel>();
            if (_model.Parameters != null && _model.Parameters.Count > 0)
            {
                foreach (var parameter in _model.Parameters)
                {
                    this.Parameters.Add(new ParameterViewModel((Parameter)parameter));
                }
            }
        }
        public string Keyword { get { return _model.Keyword; } }
        public string Name
        {
            get { return string.IsNullOrEmpty(_model.Name) ? "<procedure name>" : _model.Name; }
            set { _model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public ObservableCollection<SyntaxNodeViewModel> Parameters { get; private set; }
        public SyntaxNodes Statements { get { return _model.Statements; } }



        public void AddParameter()
        {
            Parameter p = new Parameter();
            _model.Parameters.Add(p);
            this.Parameters.Add(new ParameterViewModel(p));
        }
        public void AddSelectStatement()
        {
            this.Statements.Add(new SelectStatement()
            {
                Alias = "<type statement's alias here>"
            });
        }
    }
}
