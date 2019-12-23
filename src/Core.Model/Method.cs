using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public class Method : Entity, IHaveChildren
    {
        [PropertyPurpose(PropertyPurpose.Hierarchy)] public ComplexEntity Owner { get; set; }
        public Entity ReturnType { get; set; }
        [PropertyPurpose(PropertyPurpose.Children)] public List<Parameter> Parameters { get; } = new List<Parameter>();
        public void AddChild(Entity child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (child is Parameter)
            {
                Add((Parameter)child);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(child));
            }
        }
        private void Add(Parameter parameter)
        {
            if (Parameters.Contains(parameter)) return;
            if (Parameters.Where(i => i.Name == parameter.Name).FirstOrDefault() != null) return;
            parameter.Owner = this;
            Parameters.Add(parameter);
        }
    }
}