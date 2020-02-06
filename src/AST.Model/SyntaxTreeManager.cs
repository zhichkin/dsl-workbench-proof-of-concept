using System;
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

            TypeConstraint constraints = new TypeConstraint();
            ProcessTypeConstraintAttribute(constraints, property);
            Type propertyType = GetPropertyType(property);
            ClassifyTypeConstraints(constraints, propertyType);
            return constraints;
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
    }
}