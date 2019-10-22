using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class ProcedureViewModel : SyntaxNodeViewModel<Procedure>
    {
        public ProcedureViewModel() { }
        public ProcedureViewModel(Procedure model) : base(model) { }
        public string Keyword { get { return _model.Keyword; } }
        public string Name
        {
            get { return string.IsNullOrEmpty(_model.Name) ? "<type procedure name here>" : _model.Name; }
            set { _model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public SyntaxNodes Parameters { get { return _model.Parameters; } }
        public SyntaxNodes Statements { get { return _model.Statements; } }

        public void AddParameter()
        {
            this.Parameters.Add(new ProcedureParameter()
            {
                Type = typeof(int),
                Name = "<type parameter name here>",
                Value = 0
            });
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
