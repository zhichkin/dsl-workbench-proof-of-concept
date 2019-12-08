using Newtonsoft.Json.Serialization;
using OneCSharp.DSL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.DSL.Services
{
    //public sealed class JsonSerializationBinder : ISerializationBinder
    //{
    //    public IList<Type> KnownTypes { get; set; }

    //    public JsonSerializationBinder()
    //    {
    //        KnownTypes = new List<Type>()
    //        {
    //            typeof(BooleanOperator),
    //            typeof(ComparisonOperator),
    //            typeof(Entity),
    //            typeof(JoinExpression),
    //            typeof(ParameterExpression),
    //            typeof(PropertyExpression),
    //            typeof(Property),
    //            typeof(PropertyReference),
    //            typeof(QueryExpression),
    //            typeof(ReferenceProxy),
    //            typeof(SelectStatement),
    //            typeof(TableExpression)
    //        };
    //    }

    //    public Type BindToType(string assemblyName, string typeName)
    //    {
    //        return KnownTypes.SingleOrDefault(t => t.Name == typeName);
    //    }

    //    public void BindToName(Type serializedType, out string assemblyName, out string typeName)
    //    {
    //        assemblyName = null;
    //        typeName = serializedType.Name;
    //    }
    //}
}
