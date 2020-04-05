using OneCSharp.Integrator.Model;
using System;
using System.IO;
using System.Reflection;

namespace OneCSharp.Integrator.Services
{
    public sealed class ContractsService
    {
        private readonly JsonContractSerializer _serializer = new JsonContractSerializer();
        public Assembly LoadJsonSchema(string catalogPath)
        {
            DirectoryInfo root = new DirectoryInfo(catalogPath);
            if (!root.Exists)
            {
                throw new DirectoryNotFoundException(catalogPath);
            }
            DummyAssembly contract = new DummyAssembly();
            VisitDirectory(root, contract);
            return null;
        }
        private void VisitDirectory(DirectoryInfo parent, DummyAssembly contract)
        {
            foreach (FileInfo file in parent.GetFiles("*.json"))
            {
                VisitFile(file, contract);
            }
            foreach (DirectoryInfo child in parent.GetDirectories())
            {
                VisitDirectory(child, contract);
            }
        }
        private void VisitFile(FileInfo file, DummyAssembly contract)
        {
            string json = File.ReadAllText(file.FullName);
            DummyAssembly dummy = _serializer.FromJson(json);
        }
    }
}

//Type baseType;
//TreeNodeViewModel currentNode;
//Stack<Type> stack = new Stack<Type>();

//            foreach (Type type in types)
//            {
//                baseType = type;
//                currentNode = root;

//                while (baseType != null)
//                {
//                    if (baseType == rootType)
//                    {
//                        stack.Push(baseType);
//                        break;
//                    }
//                    stack.Push(baseType);
//                    baseType = baseType.BaseType;
//                }

//                while (stack.Count > 0)
//                {
//                    baseType = stack.Pop();
//                    currentNode = AddNodeToTypeSelectorTree(currentNode, baseType);
//                }
//            }