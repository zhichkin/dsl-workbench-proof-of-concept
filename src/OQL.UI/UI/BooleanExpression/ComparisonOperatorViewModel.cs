using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Services;
using System.ComponentModel;

namespace OneCSharp.OQL.UI
{
    public sealed class ComparisonOperatorViewModel : SyntaxNodeViewModel
    {
        private ISyntaxNodeViewModel _LeftExpression;
        private ISyntaxNodeViewModel _RightExpression;
        public ComparisonOperatorViewModel(SyntaxNodeViewModel parent, ComparisonOperator model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            ComparisonOperator model = Model as ComparisonOperator;
            if (model.LeftExpression == null)
            {
                model.LeftExpression = new PropertyReference(model);
                LeftExpression = new PropertyReferenceViewModel(this, (PropertyReference)model.LeftExpression);
            }
            else
            {
                LeftExpression = UIServices.CreateViewModel(this, model.LeftExpression);
            }
            if (model.RightExpression == null)
            {
                model.RightExpression = new PropertyReference(model);
                RightExpression = new PropertyReferenceViewModel(this, (PropertyReference)model.RightExpression);
            }
            else
            {
                RightExpression = UIServices.CreateViewModel(this, model.RightExpression);
            }
        }
        public string Literal
        {
            get { return ((ComparisonOperator)Model).Literal; }
            set { ((ComparisonOperator)Model).Literal = value; OnPropertyChanged(nameof(Literal)); }
        }
        public ISyntaxNodeViewModel LeftExpression
        {
            get { return _LeftExpression; }
            set { _LeftExpression = value; OnPropertyChanged(nameof(LeftExpression)); }
        }
        public ISyntaxNodeViewModel RightExpression
        {
            get { return _RightExpression; }
            set { _RightExpression = value; OnPropertyChanged(nameof(RightExpression)); }
        }
        public void SelectComparisonOperator()
        {
            UIServices.OpenComparisonOperatorSelectionPopup(ComparisonOperatorSelected);
        }
        private void ComparisonOperatorSelected(string operatorLiteral)
        {
            if (string.IsNullOrWhiteSpace(operatorLiteral)) return;
            Literal = operatorLiteral;
        }
    

        public void ComparisonExpressionSelected(ISyntaxNodeViewModel sender, ISyntaxNode selection)
        {
            if (selection is PropertyObject)
            {
                PropertySelected(sender, (PropertyObject)selection);
            }
            else if (selection is Parameter)
            {
                ParameterSelected(sender, (Parameter)selection);
            }
        }
        private void PropertySelected(ISyntaxNodeViewModel sender, PropertyObject property)
        {
            ComparisonOperator parent = Model as ComparisonOperator;
            var alias = GetTableSource((TableObject)property.Parent);

            PropertyReference model = new PropertyReference(parent)
            {
                TableSource = alias.Model,
                PropertySource = property
            };

            if (LeftExpression == sender)
            {
                parent.LeftExpression = model;
                LeftExpression = UIServices.CreateViewModel(this, parent.LeftExpression);
                alias.PropertyChanged += LeftAlias_PropertyChanged; //TODO: use WeakEventManager !
            }
            else if (RightExpression == sender)
            {
                parent.RightExpression = model;
                RightExpression = UIServices.CreateViewModel(this, parent.RightExpression);
                alias.PropertyChanged += RightAlias_PropertyChanged; //TODO: use WeakEventManager !
            }
        }
        private void ParameterSelected(ISyntaxNodeViewModel sender, Parameter parameter)
        {
            ComparisonOperator model = Model as ComparisonOperator;
            var viewModel = GetParameterViewModel(parameter);

            if (LeftExpression == sender)
            {   
                model.LeftExpression = parameter;
                LeftExpression = UIServices.CreateViewModel(this, model.LeftExpression);
                viewModel.PropertyChanged += LeftName_PropertyChanged; //TODO: use WeakEventManager !
            }
            else if (RightExpression == sender)
            {
                model.RightExpression = parameter;
                RightExpression = UIServices.CreateViewModel(this, model.RightExpression);
                viewModel.PropertyChanged += RightName_PropertyChanged; //TODO: use WeakEventManager !
            }
        }
        private AliasSyntaxNodeViewModel GetTableSource(TableObject table)
        {
            FromClauseViewModel from = UIServices.GetParent<FromClauseViewModel>(this);
            if (from == null) return null;

            AliasSyntaxNodeViewModel alias = null;
            foreach (var item in from)
            {
                if (table == UIServices.GetTableObject(item))
                {
                    if (item is AliasSyntaxNodeViewModel)
                    {
                        alias = (AliasSyntaxNodeViewModel)item;
                    }
                    else if (item is JoinOperatorViewModel)
                    {
                        alias = ((JoinOperatorViewModel)item).Expression as AliasSyntaxNodeViewModel;
                    }
                    break;
                }
            }
            return alias;
        }
        private void LeftAlias_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Alias" && LeftExpression != null)
            {
                ((SyntaxNodeViewModel)LeftExpression).InitializeViewModel();
            }
        }
        private void RightAlias_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Alias" && RightExpression != null)
            {
                ((SyntaxNodeViewModel)RightExpression).InitializeViewModel();
            }
        }
        private ParameterViewModel GetParameterViewModel(Parameter parameter)
        {
            ProcedureViewModel procedure = UIServices.GetParent<ProcedureViewModel>(this);
            if (procedure == null) return null;

            ParameterViewModel viewModel = null;
            foreach (var item in procedure.Parameters)
            {
                if (((ParameterViewModel)item).Model == parameter)
                {
                    viewModel = (ParameterViewModel)item;
                    break;
                }
            }
            return viewModel;
        }
        private void LeftName_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name" && LeftExpression != null)
            {
                ((SyntaxNodeViewModel)LeftExpression).InitializeViewModel();
            }
        }
        private void RightName_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name" && RightExpression != null)
            {
                ((SyntaxNodeViewModel)RightExpression).InitializeViewModel();
            }
        }
    }
}
