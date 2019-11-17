﻿using OneCSharp.Metadata;
using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace OneCSharp.OQL.UI.Services
{
    public static class UIServices
    {
        public static TParent GetParent<TParent>(ISyntaxNode child) where TParent : ISyntaxNode
        {
            ISyntaxNode parent = child.Parent;
            while (parent != null && parent.GetType() != typeof(TParent))
            {
                parent = parent.Parent;
            }
            return (TParent)parent;
        }
        public static TParent GetParent<TParent>(ISyntaxNodeViewModel child) where TParent : ISyntaxNodeViewModel
        {
            ISyntaxNodeViewModel parent = child.Parent;
            while (parent != null && parent.GetType() != typeof(TParent))
            {
                parent = parent.Parent;
            }
            return (TParent)parent;
        }
        public static MetadataProvider GetMetadataProvider(ISyntaxNodeViewModel viewModel)
        {
            ProcedureViewModel parent = GetParent<ProcedureViewModel>(viewModel);
            if (parent == null) return null;
            return parent.Metadata;
        }



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
            if (_TypeSelectionPopup != null) CloseTypeSelectionPopup();

            TreeNodeViewModel model = GetTypesTreeForSelection();
            _TypeSelectionDialog = new TypeSelectionDialog(model);
            _TypeSelectionPopup = new Popup
            {
                Placement = PlacementMode.Bottom,
                AllowsTransparency = true,
                Child = _TypeSelectionDialog
            };
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


        private static Popup _popup;
        private static Action<string> _callback;
        private static Action<object> _callbackObject;
        public static void OpenJoinTypeSelectionPopup(Action<string> callback)
        {
            _callback = callback;

            ListView selectionList = new ListView() { ItemsSource = JoinTypes.JoinTypesList };
            selectionList.SelectionChanged += JoinType_Selected;

            _popup = new Popup
            {
                Placement = PlacementMode.Mouse,
                AllowsTransparency = true,
                Child = selectionList
            };
            _popup.IsOpen = true;
        }
        private static void JoinType_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (_popup == null) return;

            _popup.IsOpen = false;
            _popup = null;

            if (sender is ListView view)
            {
                _callback((string)view.SelectedItem);
            }
            _callback = null;
            e.Handled = true;
        }
        public static void OpenHintTypeSelectionPopup(Action<string> callback)
        {
            _callback = callback;

            ListView selectionList = new ListView() { ItemsSource = HintTypes.HintTypesList };
            selectionList.SelectionChanged += HintType_Selected;

            _popup = new Popup
            {
                Placement = PlacementMode.Mouse,
                AllowsTransparency = true,
                Child = selectionList
            };
            _popup.IsOpen = true;
        }
        private static void HintType_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (_popup == null) return;

            _popup.IsOpen = false;
            _popup = null;

            if (sender is ListView view)
            {
                _callback((string)view.SelectedItem);
            }
            _callback = null;
            e.Handled = true;
        }
        public static void OpenComparisonOperatorSelectionPopup(Action<string> callback)
        {
            _callback = callback;

            ListView selectionList = new ListView() { ItemsSource = ComparisonOperators.ComparisonOperatorsList };
            selectionList.SelectionChanged += ComparisonOperator_Selected; // TODO: use WeakEventManager class

            _popup = new Popup
            {
                Placement = PlacementMode.Mouse,
                AllowsTransparency = true,
                Child = selectionList
            };
            _popup.IsOpen = true;
        }
        private static void ComparisonOperator_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (_popup == null) return;

            _popup.IsOpen = false;
            _popup = null;

            if (sender is ListView view)
            {
                _callback((string)view.SelectedItem);
                view.SelectionChanged -= ComparisonOperator_Selected; // TODO: use WeakEventManager class
            }
            _callback = null;
            e.Handled = true;
        }
        public static void OpenPropertyReferenceSelectionPopup(ISyntaxNode caller, Action<object> callback)
        {
            var root = new TreeNodeViewModel(null, "");
            var parameters = ParametersToSelect(caller);
            var properties = PropertiesToSelect(caller);
            if (parameters == null && properties == null) return;

            if (parameters != null)
            {
                parameters.Parent = root;
                root.Children.Add(parameters);
            }

            if (properties != null)
            {
                foreach (var child in properties.Children)
                {
                    child.Parent = root;
                    root.Children.Add(child);
                }
            }

            _callbackObject = callback;
            TypeSelectionDialog dialog = new TypeSelectionDialog(root);
            dialog.OnSelectionChanged = PropertyReference_Selected;

            _popup = new Popup
            {
                Placement = PlacementMode.Mouse,
                AllowsTransparency = true,
                Child = dialog
            };
            _popup.IsOpen = true;
        }
        private static void PropertyReference_Selected(TreeNodeViewModel selectedItem)
        {
            if (_popup == null) return;

            _popup.IsOpen = false;
            _popup = null;

            _callbackObject(selectedItem);
            _callbackObject = null;
        }
        private static TreeNodeViewModel ParametersToSelect(ISyntaxNode caller)
        {
            Procedure procedure = GetParent<Procedure>(caller);
            if (procedure == null || procedure.Parameters == null || procedure.Parameters.Count == 0) return null;

            var root = new TreeNodeViewModel(null, "Parameters");
            foreach (Parameter parameter in procedure.Parameters)
            {
                var child = new TreeNodeViewModel(root, $"@{parameter.Name} ({GetTypeName(parameter.Type)})", parameter);
                root.Children.Add(child);
            }
            return root;
        }
        private static TreeNodeViewModel PropertiesToSelect(ISyntaxNode caller)
        {
            JoinOperator parent = GetParent<JoinOperator>(caller);
            if (parent == null) return null;
            if (!(parent.Parent is FromClauseSyntaxNode from)) return null;

            List<ISyntaxNode> tables = new List<ISyntaxNode>();
            int index = from.IndexOf(parent);
            for (int i = 0; i <= index; i++)
            {
                tables.Add(from[i]);
            }
            // TODO: inspect FROM clause to contain SelectStatement
            if (tables.Count == 0) return null;

            var root = new TreeNodeViewModel(null, "");
            foreach (ISyntaxNode table in tables)
            {
                string caption = string.Empty;
                AliasSyntaxNode payload = null;
                if (table is AliasSyntaxNode alias)
                {
                    caption = alias.Alias;
                    payload = alias;
                }
                else if (table is JoinOperator join)
                {
                    caption = ((AliasSyntaxNode)join.Expression).Alias;
                    payload = (AliasSyntaxNode)join.Expression;
                }
                var tableNode = new TreeNodeViewModel(root, caption, payload);
                root.Children.Add(tableNode);

                TableObject source = ((HintSyntaxNode)payload.Expression).Expression as TableObject;
                foreach (DbProperty dbp in source.Table.Properties)
                {
                    PropertyObject property = new PropertyObject(source) { Property = dbp };
                    tableNode.Children.Add(new TreeNodeViewModel(tableNode, dbp.Name, property));
                }
            }

            return root.Children.Count == 0 ? null : root;
        }

        #region " SyntaxNode ViewModel Factory"
        public static ISyntaxNodeViewModel CreateViewModel(ISyntaxNodeViewModel parent, ISyntaxNode model)
        {
            if (model is Parameter)
            {
                return new ParameterReferenceViewModel(parent, (Parameter)model);
            }
            else if (model is PropertyReference)
            {
                return new PropertyReferenceViewModel(parent, (PropertyReference)model);
            }
            else if (model is PropertyObject)
            {
                return new PropertyObjectViewModel(parent, (PropertyObject)model);
            }
            return null;
        }
        #endregion

        public static TableObject GetTableObject(ISyntaxNodeViewModel viewModel)
        {
            if (viewModel == null) return null;

            TableObject table = null;
            if (viewModel is JoinOperatorViewModel)
            {
                table = ((JoinOperatorViewModel)viewModel).Expression.Model as TableObject;
                if (table == null)
                {
                    return GetTableObject(((JoinOperatorViewModel)viewModel).Expression);
                }
                else
                {
                    return table;
                }
            }
            else if (viewModel is AliasSyntaxNodeViewModel)
            {
                table = ((AliasSyntaxNodeViewModel)viewModel).Expression.Model as TableObject;
                if (table == null)
                {
                    return GetTableObject(((AliasSyntaxNodeViewModel)viewModel).Expression);
                }
                else
                {
                    return table;
                }
            }
            else if (viewModel is HintSyntaxNodeViewModel)
            {
                table = ((HintSyntaxNodeViewModel)viewModel).Expression.Model as TableObject;
                if (table == null)
                {
                    return GetTableObject(((HintSyntaxNodeViewModel)viewModel).Expression);
                }
                else
                {
                    return table;
                }
            }
            return table;
        }
    }
}
