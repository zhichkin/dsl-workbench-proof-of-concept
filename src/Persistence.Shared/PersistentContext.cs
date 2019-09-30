using System;

namespace OneCSharp.Persistence.Shared
{
    public interface IPersistentContext
    {
        IDataPersister GetDataPersister(Type type);
        IDataPersister GetDataPersister(int typeCode);
        IObjectFactory GetObjectFactory(Type type);
        IObjectFactory GetObjectFactory(int typeCode);
    }
}
