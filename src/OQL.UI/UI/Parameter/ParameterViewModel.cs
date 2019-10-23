using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class ParameterViewModel : SyntaxNodeViewModel
    {
        private readonly Parameter _model;
        public ParameterViewModel() { _model = new Parameter(); }
        public ParameterViewModel(Parameter model) { _model = model; }
        public string Name
        {
            get { return string.IsNullOrEmpty(_model.Name) ? "<parameter name>" : _model.Name; }
            set { _model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public string TypeName { get { return _model.Type.Name; } }
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
    }
}
