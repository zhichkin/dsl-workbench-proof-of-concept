using System;

namespace OneCSharp.Persistence.Shared
{
    public interface IDataPersister
    {
        IPersistentContext Context { get; }
        void Select(IPersistentObject persistentObject);
        void Insert(IPersistentObject persistentObject);
        void Update(IPersistentObject persistentObject);
        void Delete(IPersistentObject persistentObject);
    }
}
