using OneCSharp.Metadata.Shared;
using OneCSharp.Persistence.Shared;
using System;
using System.Collections.Generic;

namespace OneCSharp.Metadata.Server
{
    public sealed class MetadataObjectFactory : IObjectFactory
    {
        private readonly Dictionary<int, Func<ReferenceObject>> _factoryMethods = new Dictionary<int, Func<ReferenceObject>>();
        public MetadataObjectFactory()
        {
            _factoryMethods.Add(1, () => { return new InfoBase(); });
            _factoryMethods.Add(2, () => { return new Namespace(); });
            _factoryMethods.Add(3, () => { return new Entity(); });
            _factoryMethods.Add(4, () => { return new Property(); });
            _factoryMethods.Add(5, () => { return new Table(); });
            _factoryMethods.Add(6, () => { return new Field(); });
            _factoryMethods.Add(7, () => { return new Relation(); });
            _factoryMethods.Add(8, () => { return new Query(); });
        }
        public IDataTransferObject New(Type type)
        {
            TypeCodeAttribute tca = (TypeCodeAttribute)type.GetCustomAttributes(typeof(TypeCodeAttribute), false)[0];
            return this.New(tca.TypeCode);
        }
        public IDataTransferObject New(int typeCode)
        {
            return _factoryMethods[typeCode]();
        }
        public IDataTransferObject New(Type type, object key)
        {
            var entity = (MetadataObject)this.New(type);
            var po = entity as IPersistentObject<Guid>;
            po.PrimaryKey = (Guid)key;
            return entity;
        }
        public IDataTransferObject New(int typeCode, object key)
        {
            var entity = (MetadataObject)this.New(typeCode);
            var po = entity as IPersistentObject<Guid>;
            po.PrimaryKey = (Guid)key;
            return entity;
        }
    }
}
