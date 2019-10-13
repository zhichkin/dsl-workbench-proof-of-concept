using System;

namespace OneCSharp.Persistence.Shared
{
    public interface IDataPersister
    {
        IPersistentContext Context { get; set; }
        int Insert(ref ReferenceObject dto);
        int Update(ref ReferenceObject dto);
        int Delete(ref ReferenceObject dto);
        int Select(ref ReferenceObject dto);
    }
}
