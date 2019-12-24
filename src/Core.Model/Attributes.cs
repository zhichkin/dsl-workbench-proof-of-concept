using System;

namespace OneCSharp.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class HierarchyAttribute : Attribute
    {
        public HierarchyAttribute() { }
    }
}