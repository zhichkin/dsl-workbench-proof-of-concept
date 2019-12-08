using OneCSharp.Metadata.Model;
using System.Collections.Generic;

namespace OneCSharp.DSL.Model
{
    public sealed class TableObject : SyntaxNode
    {
        public TableObject(ISyntaxNode parent) : base(parent) { }
        public IEntity Table { get; set; }
        public string FullName
        {
            get
            {
                string fullName = Table.Parent.Domain.Database;
                List<IEntity> owners = new List<IEntity>();
                List<INamespace> parents = new List<INamespace>();

                INamespace parent = Table.Parent;
                while (parent != null)
                {
                    parents.Add(parent);
                    parent = parent.Parent;
                }
                parents.Reverse();
                foreach (INamespace item in parents)
                {
                    fullName += "." + item.Name;
                }

                IEntity owner = Table.Owner;
                while (owner != null)
                {
                    owners.Add(owner);
                    owner = owner.Owner;
                }
                owners.Reverse();
                foreach (IEntity item in owners)
                {
                    fullName += "." + item.Name;
                }

                return fullName + "." + Table.Name;
            }
        }
    }
}
