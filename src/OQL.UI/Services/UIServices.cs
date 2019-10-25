using OneCSharp.OQL.UI.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace OneCSharp.OQL.UI.Services
{
    public static class UIServices
    {
        private static Popup _TypeSelectionPopup;
        private static TypeSelectionDialog _TypeSelectionDialog;
        private static TreeNodeViewModel GetTypesTreeForSelection()
        {
            var root = new TreeNodeViewModel(null, "Simple types");
            root.Children.Add(new TreeNodeViewModel(root, "UUID", typeof(Guid)));
            root.Children.Add(new TreeNodeViewModel(root, "Binary", typeof(byte[])));
            root.Children.Add(new TreeNodeViewModel(root, "String", typeof(string)));
            root.Children.Add(new TreeNodeViewModel(root, "Numeric", typeof(int)));
            root.Children.Add(new TreeNodeViewModel(root, "Boolean", typeof(bool)));
            root.Children.Add(new TreeNodeViewModel(root, "DateTime", typeof(DateTime)));
            return root;
        }
        public static void OpenTypeSelectionPopup(UIElement target, Action<TreeNodeViewModel> callback)
        {
            if (_TypeSelectionPopup == null)
            {
                TreeNodeViewModel model = GetTypesTreeForSelection();
                _TypeSelectionDialog = new TypeSelectionDialog(model);
                _TypeSelectionPopup = new Popup
                {
                    Placement = PlacementMode.Bottom,
                    AllowsTransparency = true,
                    Child = _TypeSelectionDialog
                };
            }
            _TypeSelectionPopup.PlacementTarget = target;
            _TypeSelectionDialog.OnSelectionChanged = callback;
            _TypeSelectionPopup.IsOpen = true;
        }
        public static void CloseTypeSelectionPopup()
        {
            if (_TypeSelectionPopup == null) return;
            _TypeSelectionPopup.IsOpen = false;
            if (_TypeSelectionDialog == null) return;
            _TypeSelectionDialog.OnSelectionChanged = null;
        }

        private static readonly Dictionary<Type, string> _TypeNames = new Dictionary<Type, string>()
        {
            { typeof(Guid), "UUID" },
            { typeof(byte[]), "Binary" },
            { typeof(string), "String" },
            { typeof(int), "Numeric" },
            { typeof(bool), "Boolean" },
            { typeof(DateTime), "DateTime" },
            { typeof(long), "Numeric" },
            { typeof(decimal), "Numeric" },
            { typeof(float), "Numeric" },
            { typeof(byte), "Numeric" },
            { typeof(uint), "Numeric" },
            { typeof(sbyte), "Numeric" },
            { typeof(short), "Numeric" },
            { typeof(ushort), "Numeric" }
        };
        public static string GetTypeName(Type type)
        {
            string name = string.Empty;
            if (_TypeNames.TryGetValue(type, out name))
            {
                return name;
            }
            return "Unknown";
        }
    }
}
