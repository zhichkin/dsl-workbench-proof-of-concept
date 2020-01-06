using System.Collections.Generic;

namespace OneCSharp.Core.Model
{
    public sealed class ListType : DataType
    {
        public List<ComplexType> Type { get; } = new List<ComplexType>();
    }
}