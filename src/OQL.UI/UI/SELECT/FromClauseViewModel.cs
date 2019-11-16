using OneCSharp.Metadata;
using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Dialogs;
using OneCSharp.OQL.UI.Services;
using System.Windows.Media;

namespace OneCSharp.OQL.UI
{
    public sealed class FromClauseViewModel : SyntaxNodeListViewModel
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
            Tables = new SyntaxNodeListViewModel(this);

            foreach (var table in _model)
            {
                if (table is AliasSyntaxNode)
                {
                    Tables.Add(new AliasSyntaxNodeViewModel((AliasSyntaxNode)table) { Parent = this });
                }
                else if (table is JoinOperator)
                {
                    Tables.Add(new JoinOperatorViewModel((JoinOperator)table) { Parent = this });
                }
            }
        }
        private Brush _myBackground = Brushes.White;
        public Brush MyBackground { get { return _myBackground; } private set { _myBackground = value; NotifyPropertyChanged(nameof(MyBackground)); } }
        public void OnMouseEnter() { MyBackground = Brushes.AliceBlue; }
        public void OnMouseLeave() { MyBackground = Brushes.White; }
        public string Keyword { get { return _model.Keyword; } }
        public SyntaxNodeListViewModel Tables { get; private set; }
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

            if (_model.Count == 0)
            {
                AddTableObject(table);
            }
            else
            {
                AddJoinOperator(table);
            }
        }
        public void AddTableObject(DbObject table)
        {
            // TODO: implement SyntaxTreeBuilder !!!
            AliasSyntaxNode alias = new AliasSyntaxNode(_model) { Alias = $"T{_model.Count}" };
            _model.Add(alias);
            HintSyntaxNode hint = new HintSyntaxNode(alias);
            alias.Expression = hint;
            hint.Expression = new TableObject(hint) { Table = table };

            AliasSyntaxNodeViewModel viewModel = new AliasSyntaxNodeViewModel(alias) { Parent = this };
            Tables.Add(viewModel);
        }
        public void AddJoinOperator(DbObject table)
        {
            // TODO: implement SyntaxTreeBuilder !!!
            JoinOperator join = new JoinOperator(_model);
            AliasSyntaxNode alias = new AliasSyntaxNode(join) { Alias = $"T{_model.Count}" };
            join.Expression = alias;
            _model.Add(join);
            HintSyntaxNode hint = new HintSyntaxNode(alias);
            alias.Expression = hint;
            hint.Expression = new TableObject(hint) { Table = table };

            JoinOperatorViewModel viewModel = new JoinOperatorViewModel(join) { Parent = this };
            Tables.Add(viewModel);
        }
    }
}
