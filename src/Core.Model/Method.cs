using System.Collections.Generic;

namespace OneCSharp.Core.Model
{
    public class Method : Entity
    {
        public Interface Owner { get; set; }
        public DataType ReturnType { get; set; }
        public List<Parameter> Parameters { get; } = new List<Parameter>();
    }
}