using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public class Method : Entity
    {
        public ComplexEntity Owner { get; set; }
        public Entity ReturnType { get; set; }
        public List<Parameter> Parameters { get; } = new List<Parameter>();
        public void Add(Parameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));
            if (Parameters.Contains(parameter)) return;
            if (Parameters.Where(i => i.Name == parameter.Name).FirstOrDefault() != null) return;
            parameter.Owner = this;
            Parameters.Add(parameter);
        }
    }
}