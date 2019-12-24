using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public class ComplexEntity : Entity, IHierarchy
    {
        public ComplexEntity() { }
        public Namespace Namespace { get; set; }
        public ComplexEntity Parent { get; set; }
        [Hierarchy] public List<Method> Methods { get; } = new List<Method>();
        [Hierarchy] public List<Property> Properties { get; } = new List<Property>();
        public void AddChild(Entity child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (child is Method)
            {
                Add((Method)child);
            }
            else if (child is Property)
            {
                Add((Property)child);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(child));
            }
        }
        private void Add(Method method)
        {
            if (Methods.Contains(method)) return;
            if (Methods.Where(i => i.Name == method.Name).FirstOrDefault() != null) return;
            method.Owner = this;
            Methods.Add(method);
        }
        private void Add(Property property)
        {
            if (Properties.Contains(property)) return;
            if (Properties.Where(i => i.Name == property.Name).FirstOrDefault() != null) return;
            property.Owner = this;
            Properties.Add(property);
        }
    }
}