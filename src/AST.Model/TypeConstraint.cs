using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneCSharp.AST.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class TypeConstraintAttribute : Attribute
    {
        public TypeConstraintAttribute(params Type[] types) { Types = types; }
        public Type[] Types { get; } // only classes inherited from SyntaxNode !
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class SimpleTypeConstraintAttribute : Attribute
    {
        public SimpleTypeConstraintAttribute() { }
    }
    public static class TypeConstraints
    {
        /// <summary>
        /// Searches for non-abstract subclasses of the given base type in the same assembly where the base type is declared
        /// </summary>
        private static IEnumerable<Type> GetSubclassesOfType(Type baseType)
        {
            return Assembly.GetAssembly(baseType).GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType));
        }
        /// <summary>
        /// Gets property type constraints for the given concept's property (derived from SyntaxNode only)
        /// </summary>
        public static IEnumerable<ISyntaxNode> GetPropertyTypeConstraints(ISyntaxNode concept, string propertyName)
        {
            List<ISyntaxNode> constraints = new List<ISyntaxNode>();

            Type metadata = concept.GetType();
            PropertyInfo property = metadata.GetProperty(propertyName);
            TypeConstraintAttribute typeConstraint = property.GetCustomAttribute<TypeConstraintAttribute>();
            SimpleTypeConstraintAttribute simpleTypeConstraint = property.GetCustomAttribute<SimpleTypeConstraintAttribute>();

            if (simpleTypeConstraint != null)
            {
                constraints.AddRange(SimpleTypes.References);
            }
            if (typeConstraint != null)
            {
                foreach (Type type in typeConstraint.Types)
                {
                    if (type.IsAbstract)
                    {
                        foreach (Type subclass in GetSubclassesOfType(type))
                        {
                            constraints.Add((ISyntaxNode)Activator.CreateInstance(subclass));
                        }
                    }
                    else
                    {
                        if (type.IsClass && type.IsSubclassOf(typeof(SyntaxNode)))
                        {
                            constraints.Add((ISyntaxNode)Activator.CreateInstance(type));
                        }
                    }
                }
            }

            if (property.IsOptional())
            {
                property = property.PropertyType.GetProperty("Value");
            }
            Type propertyType;
            if (property.IsList()) // looking for List<T>
            {
                propertyType = property.PropertyType.GenericTypeArguments[0];
            }
            else
            {
                propertyType = property.PropertyType;
            }

            if (propertyType.IsAbstract)
            {
                foreach (Type subclass in GetSubclassesOfType(propertyType))
                {
                    constraints.Add((ISyntaxNode)Activator.CreateInstance(subclass));
                }
            }
            else
            {
                if (propertyType.IsClass && propertyType.IsSubclassOf(typeof(SyntaxNode)))
                {
                    constraints.Add((ISyntaxNode)Activator.CreateInstance(propertyType));
                }
            }

            return constraints;
        }
    }
}