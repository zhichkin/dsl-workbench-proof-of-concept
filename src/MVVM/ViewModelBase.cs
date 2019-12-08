using System.ComponentModel;

namespace OneCSharp.MVVM
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //public TParent GetParent<TParent>(ViewModelBase child) where TParent : ViewModelBase
        //{
        //    if (child == null) return default;
        //    if (child.GetType() == typeof(TParent)) return (TParent)child;

        //    ViewModelBase parent = child.Parent;
        //    while (parent != null && parent.GetType() != typeof(TParent))
        //    {
        //        parent = parent.Parent;
        //    }
        //    return (TParent)parent;
        //}
    }
}
