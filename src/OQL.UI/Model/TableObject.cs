using OneCSharp.Metadata;
using System.Collections.Generic;

namespace OneCSharp.OQL.Model
{
    public sealed class TableObject : SyntaxNode
    {
        public TableObject(ISyntaxNode parent) : base(parent) { }
        public DbObject Table { get; set; }
        public string Name
        {
            get
            {
                string fullName = Table.Parent.InfoBase.Database;
                List<DbObject> owners = new List<DbObject>();
                List<Namespace> parents = new List<Namespace>();

                Namespace parent = Table.Parent;
                while (parent != null)
                {
                    parents.Add(parent);
                    parent = parent.Parent;
                }
                parents.Reverse();
                foreach (Namespace item in parents)
                {
                    fullName += "." + item.Name;
                }

                DbObject owner = Table.Owner;
                while (owner != null)
                {
                    owners.Add(owner);
                    owner = owner.Owner;
                }
                owners.Reverse();
                foreach (DbObject item in owners)
                {
                    fullName += "." + item.Name;
                }

                return fullName + "." + Table.Name;
            }
        }
    }
}
