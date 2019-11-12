using OneCSharp.Metadata;
using System.Collections.Generic;

namespace OneCSharp.OQL.Model
{
    public abstract class TableExpression : SyntaxNode
    {
        public TableExpression(ISyntaxNode parent, DbObject metadata) : base(parent)
        {
            Metadata = metadata;
        }
        public DbObject Metadata { get; protected set; }
        public string Name { get { return Metadata.Name; } }
        public string FullName
        {
            get
            {
                string fullName = Metadata.Parent.InfoBase.Database;
                List<DbObject> owners = new List<DbObject>();
                List<Namespace> parents = new List<Namespace>();
                
                Namespace parent = Metadata.Parent;
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

                DbObject owner = Metadata.Owner;
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

                return fullName + "." + Metadata.Name;
            }
        }
    }
    public sealed class TableSource : TableExpression
    {
        public TableSource(ISyntaxNode parent, DbObject metadata) : base(parent, metadata)
        {
            Hint = HintTypes.None;
            Alias = Metadata.Name;
            JoinType = JoinTypes.None;
        }
        public string Hint { get; set; }
        public string Alias { get; set; }
        public string JoinType { get; set; }
        //public BooleanFunction ON { get; set; }
    }
}
