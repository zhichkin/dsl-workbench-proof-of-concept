using OneCSharp.Core.Model;
using OneCSharp.MVVM;
using System;

namespace OneCSharp.AST.UI
{
    public sealed class Module : IModule
    {
        private const string MODULE_FILE = "module.json";
        private const string CATALOG_PATH = "OneCSharp.AST.UI";
        public const string SOLUTION = "pack://application:,,,/OneCSharp.AST.UI;component/images/Solution.png";
        public const string ADD_LANGUAGE = "pack://application:,,,/OneCSharp.AST.UI;component/images/AddLanguage.png";
        public const string ADD_NAMESPACE = "pack://application:,,,/OneCSharp.AST.UI;component/images/AddNamespace.png";
        public const string NAMESPACE_PUBLIC = "pack://application:,,,/OneCSharp.AST.UI;component/images/NamespacePublic.png";
        public const string CS_FILE = "pack://application:,,,/OneCSharp.AST.UI;component/images/CSFileNode.png";
        public const string EDIT_WINDOW = "pack://application:,,,/OneCSharp.AST.UI;component/images/EditWindow.png";
        public const string ADD_VARIABLE = "pack://application:,,,/OneCSharp.AST.UI;component/images/AddVariable.png";
        public const string ADD_PROPERTY = "pack://application:,,,/OneCSharp.AST.UI;component/images/AddProperty.png";
        public const string DELETE_PROPERTY = "pack://application:,,,/OneCSharp.AST.UI;component/images/DeleteProperty.png";
        public const string MODIFY_PROPERTY = "pack://application:,,,/OneCSharp.AST.UI;component/images/ModifyProperty.png";

        public IShell Shell => throw new NotImplementedException();

        public void Initialize(IShell shell)
        {
            throw new NotImplementedException();
        }

        public IController GetController<T>()
        {
            throw new NotImplementedException();
        }

        public IController GetController(Type type)
        {
            throw new NotImplementedException();
        }

        public void Persist(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}