using System.Collections.Generic;

namespace OneCSharp.Core.Model
{
    public class ComplexType : DataType
    {
        public List<Property> Properties { get; } = new List<Property>();
    }
}