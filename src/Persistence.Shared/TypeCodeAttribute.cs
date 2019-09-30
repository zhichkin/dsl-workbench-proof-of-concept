using System;

namespace OneCSharp.Persistence.Shared
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TypeCodeAttribute : Attribute
    {
        public TypeCodeAttribute(int typeCode)
        {
            this.TypeCode = typeCode;
        }
        public int TypeCode { get; private set; }
    }
}
