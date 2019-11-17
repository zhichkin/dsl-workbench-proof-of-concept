using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Dialogs;
using OneCSharp.OQL.UI.Services;

namespace OneCSharp.OQL.UI
{
    public sealed class PropertyReferenceViewModel : SyntaxNodeViewModel
    {
        private const string NAME_PLACEHOLDER = "<expression>";
        public PropertyReferenceViewModel(ISyntaxNodeViewModel parent, PropertyReference model) : base(parent, model) { }
        public override void InitializeViewModel() { OnPropertyChanged(nameof(Name)); }
        public string Name
        {
            get
            {
                PropertyReference model = Model as PropertyReference;
                if (model == null || model.TableSource == null || model.PropertySource == null) return NAME_PLACEHOLDER;

                string name = string.Empty;

                if (model.TableSource is AliasSyntaxNode alias) name = alias.Alias;
                else if (model.TableSource is TableObject table) name = table.Name;
                else if (model.TableSource is HintSyntaxNode hint) name = ((TableObject)hint.Expression).Name;

                if (model.PropertySource is AliasSyntaxNode a) name += "." + a.Alias;
                else if (model.PropertySource is PropertyObject p) name += "." + p.Name;

                return name;
            }
        }
        public void SelectPropertyReference()
        {
            UIServices.OpenPropertyReferenceSelectionPopup(Model, PropertyReferenceSelected);
        }
        private void PropertyReferenceSelected(object selectedItem)
        {
            if (!(selectedItem is TreeNodeViewModel selection)) return;

            if (Parent is ComparisonOperatorViewModel parent)
            {
                parent.ComparisonExpressionSelected(this, (ISyntaxNode)selection.Payload);
            }
        }
    }
}
