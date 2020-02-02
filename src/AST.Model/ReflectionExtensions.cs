using System;
using System.Collections.Generic;
using System.Reflection;

namespace OneCSharp.AST.Model
{
    public static class ReflectionExtensions
    {
        public static PropertyInfo GetPropertyInfo(this ISyntaxNode @this, string propertyName)
        {
            Type metadata = @this.GetType();
            PropertyInfo property = metadata.GetProperty(propertyName);
            return property;
        }
        public static bool IsOptional(this PropertyInfo @this)
        {
            return (@this.PropertyType.IsGenericType
                && @this.PropertyType.GetGenericTypeDefinition() == typeof(Optional<>));
        }
        public static bool IsRepeatable(this PropertyInfo @this)
        {
            Type valueType = null;
            if (@this.IsOptional())
            {
                valueType = @this.PropertyType.GetProperty("Value").PropertyType;
            }
            else
            {
                valueType = @this.PropertyType;
            }
            return (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>));
        }
        public static List<Type> GetRepeatableTypes(this PropertyInfo @this)
        {
            List<Type> repeatableTypes = new List<Type>();

            Type valueType;
            if (@this.IsOptional())
            {
                valueType = @this.PropertyType.GetProperty("Value").PropertyType;
            }
            else
            {
                valueType = @this.PropertyType;
            }
            if (valueType.IsGenericType
                && valueType.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type type = valueType.GenericTypeArguments[0];
                if (type == typeof(object))
                {
                    var simpleTypeConstraint = @this.GetCustomAttribute<SimpleTypeConstraintAttribute>();
                    if (simpleTypeConstraint != null)
                    {
                        repeatableTypes.AddRange(SimpleTypes.List);
                    }
                    var typeConstraint = @this.GetCustomAttribute<TypeConstraintAttribute>();
                    if (typeConstraint != null)
                    {
                        repeatableTypes.AddRange(typeConstraint.Types);
                    }
                }
                else
                {
                    if (typeof(SyntaxNode).IsAssignableFrom(type))
                    {
                        repeatableTypes.Add(type);
                    }
                }
            }
            return repeatableTypes;
        }
    }
}