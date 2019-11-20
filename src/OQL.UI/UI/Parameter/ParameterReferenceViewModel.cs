using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Dialogs;
using OneCSharp.OQL.UI.Services;
using System.ComponentModel;

namespace OneCSharp.OQL.UI
{
    public sealed class ParameterReferenceViewModel : SyntaxNodeViewModel
    {
        public ParameterReferenceViewModel(ISyntaxNodeViewModel parent, Parameter model) : base(parent, model) { }
        public override void InitializeViewModel() { OnPropertyChanged(nameof(Name)); }
        public string Name { get { return $"@{((Parameter)Model).Name}"; } }
        public void SelectParameterReference()
        {
            UIServices.OpenPropertyReferenceSelectionPopup(Parent.Model, ParameterReferenceSelected);
        }
        private void ParameterReferenceSelected(object selectedItem)
        {
            if (!(selectedItem is TreeNodeViewModel selection)) return;

            if (Parent is ComparisonOperatorViewModel parent)
            {
                parent.ComparisonExpressionSelected(this, (ISyntaxNode)selection.Payload);
            }
        }
        public void ParameterNameChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name") OnPropertyChanged(nameof(Name));
        }
    }
}
