using System.Collections.Generic;

namespace OneCSharp.Core.Model
{
    public sealed class Interface : Entity
    {
        public Namespace Owner { get; set; }
        public List<Method> Methods { get; } = new List<Method>();
    }
}