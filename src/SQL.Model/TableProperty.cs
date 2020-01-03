using OneCSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.SQL.Model
{
    public sealed class TableProperty : Property, IHierarchy
    {
        public string DbName { get; set; } // TODO: remove !?
        public List<TypeInfo> Types { get; } = new List<TypeInfo>(); // TODO: remove !?
        [Hierarchy] public List<Field> Fields { get; } = new List<Field>();
        public void AddChild(Entity child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (child is Field)
            {
                Add((Field)child);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(child));
            }
        }
        private void Add(Field field)
        {
            if (Fields.Contains(field)) return;
            if (Fields.Where(i => i.Name == field.Name).FirstOrDefault() != null) return;
            field.Owner = this;
            Fields.Add(field);
        }
    }
}