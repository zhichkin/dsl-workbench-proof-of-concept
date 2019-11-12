using OneCSharp.Metadata;
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


        private static Popup _TableSourceSelectionPopup;
        public static void OpenTableSourceSelectionPopup(MetadataProvider metadataProvider, Action<TreeNodeViewModel> callback)
        {
            TreeNodeViewModel model = GetTableSourceSelectionTree(metadataProvider);
            if (model == null) return;

            _TypeSelectionDialog = new TypeSelectionDialog(model);

            _TableSourceSelectionPopup = new Popup
            {
                Placement = PlacementMode.Mouse,
                AllowsTransparency = true,
                Child = _TypeSelectionDialog
            };
            //_TypeSelectionPopup.PlacementTarget = target;
            _TypeSelectionDialog.OnSelectionChanged = callback;
            _TableSourceSelectionPopup.IsOpen = true;
        }
        public static void CloseTableSourceSelectionPopup()
        {
            if (_TableSourceSelectionPopup == null) return;
            _TableSourceSelectionPopup.IsOpen = false;
            _TableSourceSelectionPopup = null;
            if (_TypeSelectionDialog == null) return;
            _TypeSelectionDialog.OnSelectionChanged = null;
            _TypeSelectionDialog = null;
        }
        private static TreeNodeViewModel GetTableSourceSelectionTree(MetadataProvider metadataProvider)
        {
            if (metadataProvider.Servers == null || metadataProvider.Servers.Count == 0) return null;
            if (metadataProvider.Servers[0].InfoBases == null || metadataProvider.Servers[0].InfoBases.Count == 0) return null;

            var root = new TreeNodeViewModel(null, metadataProvider.Servers[0].Address);

            foreach (InfoBase infoBase in metadataProvider.Servers[0].InfoBases)
            {
                if (IsWebServicesInfoBase(infoBase)) continue;

                var node = new TreeNodeViewModel(root, infoBase.Database, infoBase);
                root.Children.Add(node);

                foreach (Namespace ns in infoBase.Namespaces)
                {
                    var nsNode = new TreeNodeViewModel(node, ns.Name, ns);
                    node.Children.Add(nsNode);

                    foreach (DbObject dbo in ns.DbObjects)
                    {
                        var dboNode = new TreeNodeViewModel(nsNode, dbo.Name, dbo);
                        nsNode.Children.Add(dboNode);

                        foreach (DbObject nested in dbo.NestedObjects)
                        {
                            var nestedNode = new TreeNodeViewModel(dboNode, nested.Name, nested);
                            dboNode.Children.Add(nestedNode);
                        }
                    }
                }
            }
            return (root.Children.Count > 0 ? root : null);
        }
        private static bool IsWebServicesInfoBase(InfoBase infoBase)
        {
            foreach (Namespace ns in infoBase.Namespaces)
            {
                if (ns.DbObjects != null && ns.DbObjects.Count > 0) return false;
            }
            return true;
        }
    }
}
