using System.Collections.Generic;

namespace OneCSharp.Core.Model
{
    public sealed class MultipleType : DataType
    {
        public List<DataType> Types { get; } = new List<DataType>();
    }
}