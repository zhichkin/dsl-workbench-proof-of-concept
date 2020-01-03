using OneCSharp.Core;
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
                NodeText = entity.Name,
                NodePayload = entity,
                NodeIcon = new BitmapImage(GetIconUri(entityType, entity))
            };
            
            foreach (PropertyInfo property in entityType.GetProperties())
            {
                HierarchyAttribute purpose = property.GetCustomAttribute<HierarchyAttribute>();
                if (purpose != null)
                {
                    IEnumerable source = (IEnumerable)property.GetValue(entity);
                    BuildTreeNodesRecursively(source, target.TreeNodes);
                }
            }
        }
        private void BuildTreeNodesRecursively(IEnumerable entities, ObservableCollection<TreeNodeViewModel> treeNodes)
        {
            foreach (Entity entity in entities)
            {
                BuildTreeNodeRecursively(entity, out TreeNodeViewModel treeNode);
                if (treeNode != null)
                {
                    treeNodes.Add(treeNode);
                }
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
            else if (entityType == typeof(Table))
            {
                Table table = (Table)entity;
                if (table.Owner == null)
                {
                    return new Uri(Module.TABLE);
                }
                else
                {
                    return new Uri(Module.NESTED_TABLE);
                }
            }
            else if (entityType == typeof(TableProperty))
            {
                return new Uri(Module.FIELD_PUBLIC);
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