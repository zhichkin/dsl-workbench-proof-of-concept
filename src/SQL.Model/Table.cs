using OneCSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.SQL.Model
{
    public sealed class Table : ComplexEntity
    {
        public string Alias { get; set; }
        public int TypeCode { get; set; }
        public Table Owner { get; set; }
        [Hierarchy] public List<Table> Tables { get; } = new List<Table>();
        public override void AddChild(Entity child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (child is Table)
            {
                Add((Table)child);
            }
            else
            {
                base.AddChild(child);
            }
        }
        private void Add(Table table)
        {
            if (Tables.Contains(table)) return;
            if (Tables.Where(i => i.Name == table.Name).FirstOrDefault() != null) return;
            table.Owner = this;
            Tables.Add(table);
        }
    }
}