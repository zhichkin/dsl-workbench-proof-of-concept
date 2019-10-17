using System;
using System.Collections.Generic;

namespace OneCSharp.Persistence.Shared
{
    public interface IPersistentContext
    {
        string ConnectionString { get; set; }
        void AddDataPersister(Type type, IDataPersister persister);
        void AddDataPersister(int typeCode, IDataPersister persister);
        IDataPersister GetDataPersister(Type type);
        IDataPersister GetDataPersister(int typeCode);
        void Select(IDataTransferObject dto);
        void Insert(IDataTransferObject dto);
        void Update(IDataTransferObject dto);
        void Delete(IDataTransferObject dto);
    }

    public class PersistentContext : IPersistentContext
    {
        private Dictionary<int, IDataPersister> _dataPersisters = new Dictionary<int, IDataPersister>();
        public PersistentContext() { }
        public PersistentContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        public string ConnectionString { get; set; }
        public void AddDataPersister(Type type, IDataPersister persister)
        {
            TypeCodeAttribute tca = (TypeCodeAttribute)type.GetCustomAttributes(typeof(TypeCodeAttribute), false)[0];
            _dataPersisters.Add(tca.TypeCode, persister);
        }
        public void AddDataPersister(int typeCode, IDataPersister persister)
        {
            _dataPersisters.Add(typeCode, persister);
        }
        public IDataPersister GetDataPersister(Type type)
        {
            TypeCodeAttribute tca = (TypeCodeAttribute)type.GetCustomAttributes(typeof(TypeCodeAttribute), false)[0];
            return this.GetDataPersister(tca.TypeCode);
        }
        public IDataPersister GetDataPersister(int typeCode)
        {
            return _dataPersisters[typeCode];
        }
        public void Select(IDataTransferObject dto)
        {
            IDataPersister persister = this.GetDataPersister(dto.TypeCode);
            int result = persister.Select(dto);
            if (result == 0) throw new OptimisticConcurrencyException();
        }
        public void Insert(IDataTransferObject dto)
        {
            IDataPersister persister = this.GetDataPersister(dto.TypeCode);
            int result = persister.Insert(dto);
            if (result == 0) throw new OptimisticConcurrencyException();
        }
        public void Update(IDataTransferObject dto)
        {
            IDataPersister persister = this.GetDataPersister(dto.TypeCode);
            int result = persister.Update(dto);
            if (result != 1) throw new OptimisticConcurrencyException();
        }
        public void Delete(IDataTransferObject dto)
        {
            IDataPersister persister = this.GetDataPersister(dto.TypeCode);
            int result = persister.Delete(dto);
            if (result == 0) throw new OptimisticConcurrencyException();
        }
    }
}
