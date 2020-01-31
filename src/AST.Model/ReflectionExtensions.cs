using System;
using System.Reflection;

namespace OneCSharp.AST.Model
{
    public static class ReflectionExtensions
    {
        public static bool IsOptional(this PropertyInfo @this)
        {
            return (@this.PropertyType.IsGenericType
                && @this.PropertyType.GetGenericTypeDefinition() == typeof(Optional<>));
        }
    }
}