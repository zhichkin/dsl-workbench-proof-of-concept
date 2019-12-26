using OneCSharp.MVVM;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace OneCSharp.AST.UI
{
    public class SyntaxElementViewModel : INotifyPropertyChanged
    {
        public SyntaxElementViewModel()
        {
            ClickCommand = new RelayCommand(OnClick);
            MouseEnterCommand = new RelayCommand(OnMouseEnter);
            MouseLeaveCommand = new RelayCommand(OnMouseLeave);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Orientation _orientation = Orientation.Horizontal;
        public Orientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; OnPropertyChanged(nameof(Orientation)); }
        }
        public ICommand ClickCommand { get; private set; }
        protected virtual void OnClick(object parameter)
        {
            if (Orientation == Orientation.Horizontal)
            {
                Orientation = Orientation.Vertical;
            }
            else
            {
                Orientation = Orientation.Horizontal;
            }

            //SyntaxElements.Add(new )
        }

        public ObservableCollection<SyntaxElementViewModel> SyntaxElements { get; } = new ObservableCollection<SyntaxElementViewModel>();

        public ICommand MouseEnterCommand { get; private set; }
        protected virtual void OnMouseEnter(object parameter)
        {
            
        }
        public ICommand MouseLeaveCommand { get; private set; }
        protected virtual void OnMouseLeave(object parameter)
        {

        }
    }
}