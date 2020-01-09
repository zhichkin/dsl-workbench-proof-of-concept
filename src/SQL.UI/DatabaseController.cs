using OneCSharp.Core.Model;
using OneCSharp.MVVM;
using OneCSharp.SQL.Model;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace OneCSharp.SQL.UI
{
    public sealed class DatabaseController : IController
    {
        private readonly IModule _module;
        public DatabaseController(IModule module)
        {
            _module = module;
        }
        public void BuildTreeNode(Entity model, out TreeNodeViewModel treeNode)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (!(model is Database database)) throw new ArgumentOutOfRangeException(nameof(model));

            BuildTreeNodeRecursively(database, out treeNode);
            
            //BuildContextMenu(treeNode);
        }
        private void BuildTreeNodeRecursively(Entity entity, out TreeNodeViewModel target)
        {
            Type entityType = entity.GetType();

            target = new TreeNodeViewModel()
            {
                NodeText = GetStringPresentation(entityType, entity),
                NodeToolTip = GetNodeToolTip(entityType, entity),
                NodePayload = entity,
                NodeIcon = new BitmapImage(GetIconUri(entityType, entity))
            };
            
            foreach (PropertyInfo property in entityType.GetProperties())
            {
                if (property.PropertyType.IsGenericType) // List<T>
                {
                    if (property.PropertyType.GenericTypeArguments[0] == typeof(Field)) continue;
                    IEnumerable source = (IEnumerable)property.GetValue(entity);
                    BuildTreeNodesRecursively(source, target.TreeNodes); // build child nodes
                }
            }
        }
        private void BuildTreeNodesRecursively(IEnumerable entities, ObservableCollection<TreeNodeViewModel> treeNodes)
        {
            foreach (Entity entity in entities) // child nodes
            {
                BuildTreeNodeRecursively(entity, out TreeNodeViewModel treeNode);
                if (treeNode == null) continue;

                treeNodes.Add(treeNode);

                if (entity is Property property) // look for nested tables (List<T> properties)
                {
                    if (property.ValueType is ListType listType) // nested meta-object = table part
                    {
                        treeNode.NodeToolTip = GetNodeToolTip(listType.Type.GetType(), listType.Type);
                        BuildTreeNodesRecursively(listType.Type.Properties, treeNode.TreeNodes);
                    }
                }
            }
        }
        private string GetStringPresentation(Type entityType, Entity entity)
        {
            if (entityType == typeof(Database))
            {
                Database database = ((Database)entity);
                return string.IsNullOrWhiteSpace(database.Alias) ? database.Name : database.Alias;
            }
            else if (entityType == typeof(Namespace))
            {
                return entity.Name;
            }
            else if (entityType == typeof(MetaObject))
            {
                return ((MetaObject)entity).Alias;
            }
            else if (entityType == typeof(Property))
            {
                return entity.Name;
            }
            else if (entityType == typeof(Field))
            {
                return entity.Name;
            }
            else
            {
                return entity.Name;
            }
        }
        private Uri GetIconUri(Type entityType, Entity entity)
        {
            if (entityType == typeof(Database))
            {
                return new Uri(Module.DATABASE);
            }
            else if (entityType == typeof(Namespace))
            {
                return new Uri(Module.NAMESPACE_PUBLIC);
            }
            else if (entityType == typeof(MetaObject))
            {
                return new Uri(Module.TABLE);
            }
            else if (entityType == typeof(Property))
            {
                Property property = (Property)entity;
                if(property.ValueType is ListType)
                {
                    return new Uri(Module.NESTED_TABLE);
                }
                else
                {
                    return new Uri(Module.FIELD_PUBLIC);
                }
            }
            else if (entityType == typeof(Field))
            {
                return new Uri(Module.FIELD_PUBLIC);
            }
            else
            {
                return new Uri(Module.DATABASE);
            }
        }
        private string GetNodeToolTip(Type entityType, Entity entity)
        {
            if (entityType == typeof(Database))
            {
                return entity.Name;
            }
            else if (entityType == typeof(Namespace))
            {
                return null;
            }
            else if (entityType == typeof(MetaObject))
            {
                return entity.Name;
            }
            else if (entityType == typeof(Property))
            {
                string toolTip = null;
                Property property = (Property)entity;
                foreach (Field field in property.Fields)
                {
                    toolTip += (toolTip == null) ? field.Name : Environment.NewLine + field.Name;
                    toolTip += $" : {field.TypeName}";
                }
                return toolTip;
            }
            else if (entityType == typeof(Field))
            {
                return null;
            }
            else
            {
                return null;
            }
        }
        private void BuildContextMenu(TreeNodeViewModel treeNode)
        {
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add namespace...",
                MenuItemPayload = treeNode,
                //MenuItemCommand = new RelayCommand(AddNamespace),
                MenuItemIcon = new BitmapImage(new Uri(Module.ADD_NAMESPACE)),
            });
        }
    }
}