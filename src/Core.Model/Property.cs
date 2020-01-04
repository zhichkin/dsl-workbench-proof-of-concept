using System.Collections.Generic;

namespace OneCSharp.Core.Model
{
    public class Property : Entity
    {
        public ComplexType Owner { get; set; }
        public DataType ValueType { get; set; }
        public bool IsOptional { get; set; }
        public bool IsNestedSet { get; set; }
        public string ForeignKey { get; set; }
        public List<Field> Fields { get; set; } = new List<Field>();
    }
}