using System;

namespace OneCSharp.AST.UI
{
    public sealed class CreateRepeatableOption : SyntaxNodeViewModel
    {
        private string _presentation;
        public CreateRepeatableOption(RepeatableViewModel owner) : base(owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            _presentation = owner.PropertyBinding;
        }
        public string Presentation
        {
            get { return _presentation; }
            set { _presentation = value; OnPropertyChanged(nameof(Presentation)); }
        }
    }
}