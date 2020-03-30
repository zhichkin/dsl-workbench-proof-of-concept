using OneCSharp.AST.Model;
using System;

namespace OneCSharp.DML.Model
{
    public sealed class TableConcept : ComplexDataType, IIdentifiable
    {
        private const string PLACEHOLDER = "<table>";
        public TableConcept() { Identifier = PLACEHOLDER; }
        [TypeConstraint(typeof(ComplexDataType))] public Type TableDefinition { get; set; }
        public Optional<TableHints> Hint { get; set; } = new Optional<TableHints>();
    }
    public enum TableHints
    {
        NO_LOCK,
        ROW_LOCK,
        READ_PAST,
        READ_COMMITED,
        READ_UNCOMMITED,
        READ_REPEATABLE,
        SERIALIZABLE
    }
    public sealed class ColumnConcept : SyntaxNode
    {
        public string Name { get; set; }
        public TableConcept TableReference { get; set; }
        public override string ToString()
        {
            return (TableReference == null)
                ? Name
                : $"{TableReference.Identifier}.{Name}";
        }
    }
}