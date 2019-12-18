using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public class ComplexEntity : Entity
    {
        public Namespace Namespace { get; set; }
        public ComplexEntity Parent { get; set; }
        public List<Method> Methods { get; } = new List<Method>();
        public List<Property> Properties { get; } = new List<Property>();
        public void Add(Method method)
        {
            if (Methods.Contains(method)) return;
            if (Methods.Where(i => i.Name == method.Name).FirstOrDefault() != null) return;
            method.Owner = this;
            Methods.Add(method);
        }
        public void Add(Property property)
        {
            if (Properties.Contains(property)) return;
            if (Properties.Where(i => i.Name == property.Name).FirstOrDefault() != null) return;
            property.Owner = this;
            Properties.Add(property);
        }
    }
}