using System;
using System.Collections.Generic;
using System.Text;

namespace OneCSharp.AST.Model
{
    //public static class SimpleTypes
    //{
    //    public static readonly Type[] List = new Type[]
    //    {
    //        typeof(int),
    //        typeof(bool),
    //        typeof(string),
    //        typeof(decimal),
    //        typeof(Guid),
    //        typeof(DateTime),
    //        typeof(byte[])
    //    };
    //}

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
