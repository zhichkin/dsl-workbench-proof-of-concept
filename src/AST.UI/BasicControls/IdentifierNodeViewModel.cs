using OneCSharp.AST.Model;
using System;
using System.Linq;
using System.Windows.Input;

namespace OneCSharp.AST.UI
{
    public sealed class IdentifierNodeViewModel : SyntaxNodeViewModel
    {
        private bool _isReadOnly = false;
        private IIdentifiable _identifiable = null;
        public IdentifierNodeViewModel(ISyntaxNodeViewModel owner, ISyntaxNode model) : base(owner, model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            Type metadata = model.GetType();
            if (!metadata.GetInterfaces().Contains(typeof(IIdentifiable)))
            {
                throw new InvalidOperationException("Interface \"IIdentifiable\" is not found!");
            }

            _identifiable = (IIdentifiable)model;
        }
        public string Name
        {
            get { return _identifiable.Identifier; }
            set
            {
                _identifiable.Identifier = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); }
        }
        protected override void OnMouseLeave(object parameter)
        {
            if (parameter == null) return;
            MouseEventArgs args = (MouseEventArgs)parameter;
            args.Handled = true;
            IsMouseOver = false;
        }
    }
}