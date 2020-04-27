using OneCSharp.MVVM;
using System;

namespace OneCSharp.Integrator.Module
{
    public sealed class QueryEditorViewModel : ViewModelBase
    {
        private string _queryScript = string.Empty;
        public QueryEditorViewModel(string fileFullPath)
        {
            if (string.IsNullOrWhiteSpace(fileFullPath)) throw new ArgumentNullException(nameof(fileFullPath));
            FileFullPath = fileFullPath;
        }
        public string FileFullPath { get; }
        public string QueryScript
        {
            get { return _queryScript; }
            set { _queryScript = value; OnPropertyChanged(nameof(QueryScript)); }
        }
    }
}