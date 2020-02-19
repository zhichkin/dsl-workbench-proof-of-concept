using OneCSharp.MVVM;
using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class RemoveConceptViewModel : SyntaxNodeViewModel
    {
        private const string COMMAND_ICON = "pack://application:,,,/OneCSharp.AST.UI;component/images/Delete.png";
        public RemoveConceptViewModel(ConceptNodeViewModel owner) : base(owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            IntializeViewModel();
        }
        private void IntializeViewModel()
        {
            ExecuteCommand = new RelayCommand(Execute);
        }
        public BitmapImage CommandIcon { get; } = new BitmapImage(new Uri(COMMAND_ICON));
        public string CommandToolTip { get { return $"Remove option \"{PropertyBinding}\""; } }
        public ICommand ExecuteCommand { get; set; }
        private void Execute(object parameter)
        {
            if (!(this.Ancestor<ConceptNodeViewModel>() is ConceptNodeViewModel concept)) return;
            ((ConceptNodeViewModel)Owner).HideCommands();
            if (Owner.Owner is RepeatableViewModel repeatable)
            {
                concept.RemoveConcept(repeatable.PropertyBinding);
            }
        }
    }
}