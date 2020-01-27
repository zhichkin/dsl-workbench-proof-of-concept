using System.Collections.Generic;

namespace OneCSharp.Core.Model
{
    public class Property : Entity
    {
        public ComplexType Owner { get; set; }
        public int Ordinal { get; set; }
        public bool IsOptional { get; set; }
        public DataType ValueType { get; set; }
        public List<Field> Fields { get; set; } = new List<Field>();
    }
}