using OneCSharp.DSL.Model;
using OneCSharp.DSL.UI.Services;

namespace OneCSharp.DSL.UI
{
    public sealed class AliasSyntaxNodeViewModel : SyntaxNodeViewModel
    {
        private ISyntaxNodeViewModel _Expression;
        public AliasSyntaxNodeViewModel(ISyntaxNodeViewModel parent, AliasSyntaxNode model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            AliasSyntaxNode model = Model as AliasSyntaxNode;
            if (model.Expression != null)
            {
                Expression = UIServices.CreateViewModel(this, model.Expression);
            }
        }
        public string Alias
        {
            get { return ((AliasSyntaxNode)Model).Alias; }
            set { ((AliasSyntaxNode)Model).Alias = value; OnPropertyChanged(nameof(Alias)); }
        }
        public ISyntaxNodeViewModel Expression
        {
            get { return _Expression; }
            set { _Expression = value; OnPropertyChanged(nameof(Expression)); }
        }
    }
}
