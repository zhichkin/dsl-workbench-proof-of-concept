using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public class Method : Entity
    {
        public ComplexEntity Owner { get; set; }
        public Entity ReturnEntity { get; set; }
        public List<Property> Parameters { get; } = new List<Property>();
        public void Add(Property parameter)
        {
            if (Parameters.Contains(parameter)) return;
            if (Parameters.Where(i => i.Name == parameter.Name).FirstOrDefault() != null) return;
            Parameters.Add(parameter);
        }
    }
}