using System;

namespace OneCSharp.Persistence.Shared
{
    public interface IDataPersister
    {
        int Insert(IDataTransferObject dto);
        int Update(IDataTransferObject dto);
        int Delete(IDataTransferObject dto);
        int Select(IDataTransferObject dto);
    }
}
