using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Dialogs;
using OneCSharp.OQL.UI.Services;
using System.ComponentModel;
using System.Windows;

namespace OneCSharp.OQL.UI
{
    public sealed class SelectClauseViewModel : SyntaxNodeListViewModel
    {
        public SelectClauseViewModel(SelectStatementViewModel parent, SelectClauseSyntaxNode model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            SelectClauseSyntaxNode model = (SelectClauseSyntaxNode)Model;
            foreach (var property in model)
            {
                Add(UIServices.CreateViewModel(this, model));
            }
        }
        public void AddProperty()
        {
            if (!(Model is SelectClauseSyntaxNode model)) return;
            if (!(model.Parent is SelectStatement select)) return;
            if (select.FROM == null || select.FROM.Count == 0)
            {
                MessageBox.Show("FROM clause is empty!", "ONE-C-SHARP");
                return;
            }
            UIServices.OpenPropertyReferenceSelectionPopup(select.FROM[select.FROM.Count - 1], PropertyReferenceSelected);
        }
        private void PropertyReferenceSelected(object selectedItem)
        {
            if (!(selectedItem is TreeNodeViewModel selection)) return;
            if (!(selection.Payload is ISyntaxNode syntaxNode)) return;

            if (syntaxNode is Parameter)
            {
                ParameterSelected((SelectStatementViewModel)Parent, (Parameter)syntaxNode);
            }
            else if (syntaxNode is PropertyObject)
            {
                PropertySelected((SelectStatementViewModel)Parent, (PropertyObject)syntaxNode);
            }
        }
        private void ParameterSelected(SelectStatementViewModel parent, Parameter parameter)
        {
            SelectClauseSyntaxNode model = Model as SelectClauseSyntaxNode;
            ParameterViewModel parentVM = UIServices.GetParameterViewModel(parent, parameter);

            AliasSyntaxNode alias = new AliasSyntaxNode(Model)
            {
                Alias = $"P{model.Count}"
            };
            model.Add(alias);
            alias.Expression = parameter;
            AliasSyntaxNodeViewModel aliasVM = (AliasSyntaxNodeViewModel)UIServices.CreateViewModel(this, alias);
            Add(aliasVM);
            //TODO: use WeakEventManager !
            ParameterReferenceViewModel childVM = aliasVM.Expression as ParameterReferenceViewModel;
            parentVM.PropertyChanged += childVM.ParameterNameChanged;
        }
        private void PropertySelected(SelectStatementViewModel parent, PropertyObject property)
        {
            AliasSyntaxNodeViewModel tableAlias = UIServices.GetTableSource(parent.FROM, (TableObject)property.Parent);

            SelectClauseSyntaxNode model = (SelectClauseSyntaxNode)Model;
            AliasSyntaxNode alias = new AliasSyntaxNode(Model)
            {
                Alias = $"P{model.Count}"
            };
            model.Add(alias);
            PropertyReference child = new PropertyReference(alias)
            {
                TableSource = tableAlias.Model,
                PropertySource = property
            };
            alias.Expression = child;
            AliasSyntaxNodeViewModel aliasVM = (AliasSyntaxNodeViewModel)UIServices.CreateViewModel(this, alias);
            Add(aliasVM);
            //TODO: use WeakEventManager !
            PropertyReferenceViewModel childVM = aliasVM.Expression as PropertyReferenceViewModel;
            tableAlias.PropertyChanged += childVM.TableAliasChanged;
        }
    }
}
