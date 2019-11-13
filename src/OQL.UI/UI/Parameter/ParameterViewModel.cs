using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Services;
using System;

namespace OneCSharp.OQL.UI
{
    public sealed class ParameterViewModel : SyntaxNodeViewModel
    {
        private readonly Parameter _model;
        public ParameterViewModel() { _model = new Parameter(); InitializeViewModel(); }
        public ParameterViewModel(Parameter model) { _model = model; InitializeViewModel(); }
        public override void InitializeViewModel() { }
        public string Name
        {
            get { return string.IsNullOrEmpty(_model.Name) ? "<parameter name>" : _model.Name; }
            set { _model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public string TypeName { get { return UIServices.GetTypeName(_model.Type); } }
        public Type Type
        {
            get { return _model.Type; }
            set { _model.Type = value; OnPropertyChanged(nameof(TypeName)); }
        }
        public bool IsInput
        {
            get { return _model.IsInput; }
            set { _model.IsInput = value; OnPropertyChanged(nameof(IsInput)); }
        }
        public bool IsOutput
        {
            get { return _model.IsOutput; }
            set { _model.IsOutput = value; OnPropertyChanged(nameof(IsOutput)); }
        }

        private bool _IsRemoveButtonVisible = false;
        public bool IsRemoveButtonVisible
        {
            get { return _IsRemoveButtonVisible; }
            set { _IsRemoveButtonVisible = value; OnPropertyChanged(nameof(IsRemoveButtonVisible)); }
        }
        public void RemoveParameter()
        {
            // TODO: child should ask parent to remove it from parent's collections !
            if (_model == null || _model.Parent == null) return;
            Procedure parent = _model.Parent as Procedure;
            if (parent == null) return;
            parent.Parameters.Remove(_model);

            ProcedureViewModel vm = this.Parent as ProcedureViewModel;
            if (vm == null) return;
            vm.RemoveParameter(this);
        }
        public void MoveParameterUp()
        {
            // TODO: child should ask parent to remove it from parent's collections !
            if (_model == null || _model.Parent == null) return;
            Procedure parent = _model.Parent as Procedure;
            if (parent == null) return;
            // TODO: parent.Parameters.Remove(_model);

            ProcedureViewModel vm = this.Parent as ProcedureViewModel;
            if (vm == null) return;
            vm.MoveParameterUp(this);
        }
        public void MoveParameterDown()
        {
            // TODO: child should ask parent to remove it from parent's collections !
            if (_model == null || _model.Parent == null) return;
            Procedure parent = _model.Parent as Procedure;
            if (parent == null) return;
            // TODO: parent.Parameters.Remove(_model);

            ProcedureViewModel vm = this.Parent as ProcedureViewModel;
            if (vm == null) return;
            vm.MoveParameterDown(this);
        }
    }
}
