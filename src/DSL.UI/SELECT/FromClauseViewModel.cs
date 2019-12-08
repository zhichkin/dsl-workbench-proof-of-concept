using OneCSharp.DSL.Model;
using OneCSharp.DSL.UI.Dialogs;
using OneCSharp.DSL.UI.Services;
using OneCSharp.Metadata.Model;
using OneCSharp.Metadata.Services;
using System.Windows.Media;

namespace OneCSharp.DSL.UI
{
    public sealed class FromClauseViewModel : SyntaxNodeListViewModel
    {
        public FromClauseViewModel(SelectStatementViewModel parent, FromClauseSyntaxNode model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            foreach (var table in (FromClauseSyntaxNode)Model)
            {
                if (table is AliasSyntaxNode)
                {
                    this.Add(new AliasSyntaxNodeViewModel(this, (AliasSyntaxNode)table));
                }
                else if (table is JoinOperator)
                {
                    this.Add(new JoinOperatorViewModel(this, (JoinOperator)table));
                }
            }
        }
        private Brush _myBackground = Brushes.White;
        public Brush MyBackground { get { return _myBackground; } private set { _myBackground = value; NotifyPropertyChanged(nameof(MyBackground)); } }
        public void OnMouseEnter() { MyBackground = Brushes.AliceBlue; }
        public void OnMouseLeave() { MyBackground = Brushes.White; }
        public string Keyword { get { return ((FromClauseSyntaxNode)Model).Keyword; } }
        public void SelectTableSource()
        {
            MetadataProvider metadata = UIServices.GetMetadataProvider(this);
            if (metadata == null) return;
            UIServices.OpenTableSourceSelectionPopup(metadata, TableSourceSelected);
        }
        private void TableSourceSelected(TreeNodeViewModel selectedNode)
        {
            UIServices.CloseTableSourceSelectionPopup();
            if (selectedNode == null) return;
            if (selectedNode.Payload == null) return;
            Entity table = selectedNode.Payload as Entity;
            if (table == null) return;

            if (((FromClauseSyntaxNode)Model).Count == 0)
            {
                AddTableObject(table);
            }
            else
            {
                AddJoinOperator(table);
            }
        }
        public void AddTableObject(Entity table)
        {
            // TODO: implement SyntaxTreeBuilder !!!
            AliasSyntaxNode alias = new AliasSyntaxNode(_model) { Alias = $"T{((FromClauseSyntaxNode)Model).Count}" };
            ((FromClauseSyntaxNode)Model).Add(alias);
            HintSyntaxNode hint = new HintSyntaxNode(alias);
            alias.Expression = hint;
            hint.Expression = new TableObject(hint) { Table = table };

            AliasSyntaxNodeViewModel viewModel = new AliasSyntaxNodeViewModel(this, alias);
            this.Add(viewModel);
        }
        public void AddJoinOperator(Entity table)
        {
            // TODO: implement SyntaxTreeBuilder !!!
            JoinOperator join = new JoinOperator(_model);
            AliasSyntaxNode alias = new AliasSyntaxNode(join) { Alias = $"T{((FromClauseSyntaxNode)Model).Count}" };
            join.Expression = alias;
            ((FromClauseSyntaxNode)Model).Add(join);
            HintSyntaxNode hint = new HintSyntaxNode(alias);
            alias.Expression = hint;
            hint.Expression = new TableObject(hint) { Table = table };

            JoinOperatorViewModel viewModel = new JoinOperatorViewModel(this, join);
            this.Add(viewModel);
        }
    }
}
