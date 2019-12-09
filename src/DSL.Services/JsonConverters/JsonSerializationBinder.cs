using OneCSharp.DSL.Model;
using OneCSharp.Metadata.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.DSL.Services
{
    public interface ISerializationBinder
    {
        Type GetType(int typeCode);
        int GetTypeCode(Type type);
    }
    public sealed class JsonSerializationBinder : ISerializationBinder
    {
        public Dictionary<int, Type> KnownTypes { get; set; } // = new Dictionary<int, Type>();

        public JsonSerializationBinder()
        {
            KnownTypes = new Dictionary<int, Type>()
            {
                { 1, typeof(HintSyntaxNode) },
                { 2, typeof(AliasSyntaxNode) },
                { 3, typeof(OnSyntaxNode) },
                { 4, typeof(WhereSyntaxNode) },
                { 5, typeof(BooleanOperator) },
                { 6, typeof(ComparisonOperator) },
                { 7, typeof(Entity) },
                { 8, typeof(JoinOperator) },
                { 9, typeof(Parameter) },
                { 10, typeof(Property) },
                { 11, typeof(PropertyObject) },
                { 12, typeof(PropertyReference) },
                { 13, typeof(Procedure) },
                { 14, typeof(SelectStatement) },
                { 15, typeof(TableObject) },
                { 16, typeof(SelectClauseSyntaxNode) },
                { 17, typeof(FromClauseSyntaxNode) }
            };
        }
        public Type GetType(int typeCode)
        {
            return KnownTypes.SingleOrDefault(i => i.Key == typeCode).Value;
        }
        public int GetTypeCode(Type type)
        {
            return KnownTypes.SingleOrDefault(i => i.Value == type).Key;
        }
    }
}
