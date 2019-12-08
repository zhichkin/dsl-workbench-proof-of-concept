using OneCSharp.DSL.Model;

namespace OneCSharp.DSL.UI
{
    public sealed class PropertyObjectViewModel : SyntaxNodeViewModel
    {
        public PropertyObjectViewModel(ISyntaxNodeViewModel parent, PropertyObject model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel() { }
        public string Name { get { return ((PropertyObject)Model).Name; } }
    }
}
