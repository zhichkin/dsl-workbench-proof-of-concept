using System;

namespace OneCSharp.AST.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class TypeConstraintAttribute : Attribute
    {
        public TypeConstraintAttribute(params Type[] types) { Types = types; }
        public Type[] Types { get; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class SimpleTypeConstraintAttribute : Attribute
    {
        public SimpleTypeConstraintAttribute() { }
    }
}
