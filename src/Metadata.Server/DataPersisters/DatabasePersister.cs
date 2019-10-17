using OneCSharp.Persistence.Shared;
using System;
using System.Linq;

namespace OneCSharp.Metadata.Server
{
    // TODO: move class to OneCSharp.Persistence.Server project in future
    public sealed class DatabasePersister
    {
        public void Build(Type type)
        {
            var dto = typeof(IDataTransferObject).IsAssignableFrom(type);

            _ = type.GetCustomAttributes(typeof(TypeCodeAttribute), false);
            _ = type.GetCustomAttributes(typeof(SchemaAttribute), true);
            _ = type.GetCustomAttributes(typeof(TableAttribute), false);
            
            var persistent = type.GetInterfaces()
                .Where((i) => i.IsGenericType
                && i.GetGenericTypeDefinition() == typeof(IPersistentObject<>))
                .FirstOrDefault();
            if (persistent != null)
            {
                _ = type.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            }

            var reference = typeof(IPersistentObject<Guid>).IsAssignableFrom(type);
            var state = typeof(IPersistentState).IsAssignableFrom(type);

            var optimistic = typeof(IVersion).IsAssignableFrom(type);
            if (optimistic)
            {
                _ = type.GetCustomAttributes(typeof(VersionAttribute), true);
            }

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var fields = property.GetCustomAttributes(typeof(FieldAttribute), false);
                foreach (var field in fields)
                {
                    var a = (FieldAttribute)field;
                }
            }
        }
    }
}
