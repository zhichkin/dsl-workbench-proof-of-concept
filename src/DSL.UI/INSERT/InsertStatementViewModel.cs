using OneCSharp.DSL.Model;
using OneCSharp.DSL.UI.Services;
using OneCSharp.MVVM;
using System.Windows;
using System.Windows.Input;

namespace OneCSharp.DSL.UI
{
    public sealed class InsertStatementViewModel : SyntaxNodeViewModel
    {
        private ISyntaxNodeViewModel _Table;
        private ISyntaxNodeViewModel _DataSource;
        public InsertStatementViewModel(ISyntaxNodeViewModel parent, InsertStatement model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            UseSelectStatementCommand = new RelayCommand(UseSelectStatement);
            InsertStatement model = (InsertStatement)Model;
            if (model.Table == null) { model.Table = new TableObject(model); }
            Table = UIServices.CreateViewModel(this, model.Table);
            if (model.DataSource != null) { DataSource = UIServices.CreateViewModel(this, model.DataSource); }
        }
        public string Keyword { get { return ((IKeyword)Model).Keyword; } }
        public ISyntaxNodeViewModel Table
        {
            get { return _Table; } set { _Table = value; OnPropertyChanged(nameof(Table)); }
        }
        public ISyntaxNodeViewModel DataSource
        {
            get { return _DataSource; } set { _DataSource = value; OnPropertyChanged(nameof(DataSource)); }
        }
        public ICommand UseSelectStatementCommand { get; private set; }
        private void UseSelectStatement(object parameter)
        {
            InsertStatement model = (InsertStatement)Model;
            if (model.DataSource != null)
            {
                MessageBox.Show("SELECT statement is already in use!", "ONE-C-SHARP");
                return;
            }
            model.DataSource = new SelectStatement(model);
            DataSource = UIServices.CreateViewModel(this, model.DataSource);
        }
    }
}
