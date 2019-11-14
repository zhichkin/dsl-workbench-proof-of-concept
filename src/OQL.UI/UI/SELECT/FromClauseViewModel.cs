using OneCSharp.Metadata;
using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Dialogs;
using OneCSharp.OQL.UI.Services;
using System.Windows.Media;

namespace OneCSharp.OQL.UI
{
    public sealed class FromClauseViewModel : SyntaxNodesViewModel
    {
        private readonly FromClauseSyntaxNode _model;
        public FromClauseViewModel(SelectStatementViewModel parent)
        {
            Parent = parent;
            _model = ((SelectStatement)parent.Model).FROM;
            InitializeViewModel();
        }
        public ISyntaxNode Model { get { return _model; } }
        public override void InitializeViewModel()
        {
            Tables = new SyntaxNodesViewModel();

            foreach (var table in _model)
            {
                //TODO: abstract TableViewModel
            }
        }
        private Brush _myBackground = Brushes.White;
        public Brush MyBackground { get { return _myBackground; } private set { _myBackground = value; OnPropertyChanged(nameof(MyBackground)); } }
        public void OnMouseEnter() { MyBackground = Brushes.AliceBlue; }
        public void OnMouseLeave() { MyBackground = Brushes.White; }
        public string Keyword { get { return _model.Keyword; } }
        public SyntaxNodesViewModel Tables { get; private set; }
        private TParent GetParent<TParent>() where TParent : SyntaxNodeViewModel
        {
            ISyntaxNodeViewModel parent = this.Parent;
            while (parent != null && parent.GetType() != typeof(TParent))
            {
                parent = parent.Parent;
            }
            return (TParent)parent;
        }
        public void SelectTableSource()
        {
            ProcedureViewModel parent = GetParent<ProcedureViewModel>();
            if (parent == null) return;
            if (parent.Metadata == null) return;
            UIServices.OpenTableSourceSelectionPopup(parent.Metadata, TableSourceSelected);
        }
        private void TableSourceSelected(TreeNodeViewModel selectedNode)
        {
            UIServices.CloseTableSourceSelectionPopup();
            if (selectedNode == null) return;
            if (selectedNode.Payload == null) return;
            DbObject table = selectedNode.Payload as DbObject;
            if (table == null) return;

            // TODO: implement SyntaxTreeBuilder !!!
            JoinOperator join = new JoinOperator(_model);
            _model.Add(join);
            AliasExpression alias = new AliasExpression(join) { Alias = $"T{_model.Count}" };
            join.Expression = alias;
            HintExpression hint = new HintExpression(alias);
            alias.Expression = hint;
            hint.Expression = new TableObject(hint) { Table = table };
            
            JoinOperatorViewModel viewModel = new JoinOperatorViewModel(join) { Parent = this };
            Tables.Add(viewModel);
        }
    }
}
