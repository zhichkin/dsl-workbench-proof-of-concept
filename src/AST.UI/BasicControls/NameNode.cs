using OneCSharp.Core.Model;

namespace OneCSharp.AST.UI
{
    public sealed class NameNode : SyntaxNode
    {
        private bool _isReadOnly = false;
        public NameNode(ISyntaxNode owner, Entity model) : base(owner, model) { }
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); }
        }
    }
}