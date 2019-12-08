using OneCSharp.DSL.Model;
using OneCSharp.DSL.UI.Services;
using System;

namespace OneCSharp.DSL.UI
{
    public sealed class ParameterViewModel : SyntaxNodeViewModel
    {
        public ParameterViewModel(ISyntaxNodeViewModel parent, Parameter model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel() { }
        public string Name
        {
            get { return string.IsNullOrEmpty(((Parameter)Model).Name) ? "<parameter name>" : ((Parameter)Model).Name; }
            set { ((Parameter)Model).Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public string TypeName { get { return UIServices.GetTypeName(((Parameter)Model).Type); } }
        public Type Type
        {
            get { return ((Parameter)Model).Type; }
            set { ((Parameter)Model).Type = value; OnPropertyChanged(nameof(TypeName)); }
        }
        public bool IsInput
        {
            get { return ((Parameter)Model).IsInput; }
            set { ((Parameter)Model).IsInput = value; OnPropertyChanged(nameof(IsInput)); }
        }
        public bool IsOutput
        {
            get { return ((Parameter)Model).IsOutput; }
            set { ((Parameter)Model).IsOutput = value; OnPropertyChanged(nameof(IsOutput)); }
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
            if (Model == null || ((Parameter)Model).Parent == null) return;
            Procedure parent = _model.Parent as Procedure;
            if (parent == null) return;
            parent.Parameters.Remove(((Parameter)Model));

            ProcedureViewModel vm = this.Parent as ProcedureViewModel;
            if (vm == null) return;
            vm.RemoveParameter(this);
        }
        public void MoveParameterUp()
        {
            // TODO: child should ask parent to remove it from parent's collections !
            if (Model == null || ((Parameter)Model).Parent == null) return;
            Procedure parent = ((Parameter)Model).Parent as Procedure;
            if (parent == null) return;
            // TODO: parent.Parameters.Remove(_model);

            ProcedureViewModel vm = this.Parent as ProcedureViewModel;
            if (vm == null) return;
            vm.MoveParameterUp(this);
        }
        public void MoveParameterDown()
        {
            // TODO: child should ask parent to remove it from parent's collections !
            if (Model == null || ((Parameter)Model).Parent == null) return;
            Procedure parent = ((Parameter)Model).Parent as Procedure;
            if (parent == null) return;
            // TODO: parent.Parameters.Remove(_model);

            ProcedureViewModel vm = this.Parent as ProcedureViewModel;
            if (vm == null) return;
            vm.MoveParameterDown(this);
        }
    }
}
