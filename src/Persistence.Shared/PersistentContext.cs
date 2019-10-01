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
        void Load(IPersistentObject persistentObject);
        void Save(IPersistentObject persistentObject);
        void Kill(IPersistentObject persistentObject);
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
        public void Load(IPersistentObject persistentObject)
        {
            StateObject so = persistentObject as StateObject;
            if (so.State == PersistentState.Deleted) throw new InvalidOperationException();

            IDataPersister persister = this.GetDataPersister(persistentObject.TypeCode);
            persister.Select(persistentObject);
        }
        public void Save(IPersistentObject persistentObject)
        {
            StateObject so = persistentObject as StateObject;
            if (so.State == PersistentState.New || so.State == PersistentState.Changed)
            {
                IDataPersister persister = this.GetDataPersister(persistentObject.TypeCode);
                if (so.State == PersistentState.New)
                {
                    persister.Insert(persistentObject);
                }
                else
                {
                    persister.Update(persistentObject);
                }
            }
        }
        public void Kill(IPersistentObject persistentObject)
        {
            StateObject so = persistentObject as StateObject;
            if (so.State == PersistentState.Original || so.State == PersistentState.Changed)
            {
                IDataPersister persister = this.GetDataPersister(persistentObject.TypeCode);
                persister.Delete(persistentObject);
            }
        }
    }
}
