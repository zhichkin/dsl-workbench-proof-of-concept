using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneCSharp.AST.Model
{
    public sealed class TypeConstraint
    {
        public readonly List<Type> Concepts = new List<Type>();
        public readonly List<Type> DataTypes = new List<Type>();
        public readonly List<Type> DotNetTypes = new List<Type>();
    }
    public static class SyntaxTreeManager
    {
        private const string OPTIONAL_VALUE = "Value";
        private static readonly Dictionary<Type, IScopeProvider> _scopeProviders = new Dictionary<Type, IScopeProvider>();
        static SyntaxTreeManager()
        {
            _scopeProviders.Add(typeof(VariableConcept), new VariableScopeProvider());
        }
        public static IScopeProvider GetScopeProvider(Type concept)
        {
            if (concept == null) throw new ArgumentNullException(nameof(concept));
            if (_scopeProviders.TryGetValue(concept, out IScopeProvider provider))
            {
                return provider;
            }
            return null;
        }
        public static ISyntaxNode Ancestor<T>(this ISyntaxNode @this)
        {
            Type ancestorType = typeof(T);
            ISyntaxNode ancestor = @this.Parent;
            while (ancestor != null)
            {
                if (ancestor.GetType() != ancestorType)
                {
                    ancestor = ancestor.Parent;
                }
                else
                {
                    break;
                }
            }
            return ancestor;
        }
        public static Type GetPropertyType(PropertyInfo property)
        {
            Type propertyType;
            PropertyInfo info = property;

            if (info.IsOptional())
            {
                info = info.PropertyType.GetProperty(OPTIONAL_VALUE);
            }

            if (info.IsList()) // looking for List<T>
            {
                propertyType = info.PropertyType.GenericTypeArguments[0];
            }
            else
            {
                propertyType = info.PropertyType;
            }

            return propertyType;
        }
        public static void ClassifyTypeConstraint(TypeConstraint constraints, Type type)
        {
            if (SimpleTypes.DotNetTypes.Contains(type))
            {
                constraints.DotNetTypes.Add(type);
            }
            else if (type.IsSubclassOf(typeof(DataType)))
            {
                constraints.DataTypes.Add(type);
            }
            else if (type.IsSubclassOf(typeof(SyntaxNode)))
            {
                constraints.Concepts.Add(type);
            }
        }
        public static IEnumerable<Type> GetSubclassesOfType(Type baseType)
        {
            return Assembly.GetAssembly(baseType).GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType));
        }
        public static TypeConstraint GetTypeConstraints(ISyntaxNode concept, string propertyName)
        {
            if (concept == null) throw new ArgumentNullException(nameof(concept));
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            Type metadata = concept.GetType();
            PropertyInfo property = metadata.GetProperty(propertyName);
            if (property == null) throw new ArgumentOutOfRangeException(nameof(property));

            return GetTypeConstraints(property);
        }
        public static TypeConstraint GetTypeConstraints(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            TypeConstraint constraints = new TypeConstraint();
            ProcessTypeConstraintAttribute(constraints, property);
            Type propertyType = GetPropertyType(property);
            ClassifyTypeConstraints(constraints, propertyType);
            ProcessSimpleTypeConstraintAttribute(constraints, property);
            return constraints;
        }
        private static void ProcessSimpleTypeConstraintAttribute(TypeConstraint constraints, PropertyInfo property)
        {
            SimpleTypeConstraintAttribute typeConstraint = property.GetCustomAttribute<SimpleTypeConstraintAttribute>();
            if (typeConstraint == null) return;
            foreach (Type type in SimpleTypes.DotNetTypes)
            {
                if (!constraints.DotNetTypes.Contains(type))
                {
                    constraints.DotNetTypes.Add(type);
                }
            }
        }
        private static void ProcessTypeConstraintAttribute(TypeConstraint constraints, PropertyInfo property)
        {
            TypeConstraintAttribute typeConstraint = property.GetCustomAttribute<TypeConstraintAttribute>();
            if (typeConstraint == null) return;
            ClassifyTypeConstraints(constraints, typeConstraint.Types);
        }
        private static void ClassifyTypeConstraints(TypeConstraint constraints, params Type[] types)
        {
            foreach (Type type in types)
            {
                if (type.IsAbstract)
                {
                    foreach (Type subclass in GetSubclassesOfType(type))
                    {
                        ClassifyTypeConstraint(constraints, subclass);
                    }
                }
                else
                {
                    ClassifyTypeConstraint(constraints, type);
                }
            }
        }
        public static void SetConceptProperty(ISyntaxNode concept, string propertyName, object value)
        {
            PropertyInfo property = concept.GetPropertyInfo(propertyName);
            if (property == null) throw new NullReferenceException($"Property \"{propertyName}\" of the \"{concept.GetType()}\" type is not found!");
            if (property.IsRepeatable()) throw new InvalidOperationException($"Property \"{propertyName}\" can not be repeatable!");

            if (property.IsOptional())
            {
                IOptional optional = (IOptional)property.GetValue(concept);
                optional.Value = value;
            }
            else
            {
                property.SetValue(concept, value);
            }
        }
        public static ISyntaxNode CreateRepeatableConcept(Type repeatable, ISyntaxNode parent, string propertyName)
        {
            ISyntaxNode concept = (ISyntaxNode)Activator.CreateInstance(repeatable);
            concept.Parent = parent;

            IList list;
            PropertyInfo property = parent.GetPropertyInfo(propertyName);
            if (property.IsOptional())
            {
                IOptional optional = (IOptional)property.GetValue(parent);
                list = (IList)optional.Value;
                if (list == null)
                {
                    Type listType = property.PropertyType.GetProperty("Value").PropertyType;
                    list = (IList)Activator.CreateInstance(listType);
                    optional.Value = list;
                }
            }
            else
            {
                list = (IList)property.GetValue(parent);
                if (list == null)
                {
                    Type listType = property.PropertyType;
                    list = (IList)Activator.CreateInstance(listType);
                    property.SetValue(parent, list);
                }
            }
            list.Add(concept);
            return concept;
        }
        public static ISyntaxNode CreateConcept(Type conceptType, ISyntaxNode parent, string propertyName)
        {
            ISyntaxNode concept = (ISyntaxNode)Activator.CreateInstance(conceptType);
            concept.Parent = parent;

            PropertyInfo property = parent.GetPropertyInfo(propertyName);
            if (property.IsOptional())
            {
                IOptional optional = (IOptional)property.GetValue(parent);
                optional.Value = concept;
            }
            else
            {
                property.SetValue(parent, concept);
            }
            return concept;
        }
    }
}