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
        void Load(ReferenceObject dto);
        void Save(ReferenceObject dto);
        void Kill(ReferenceObject dto);
        void AddObjectFactory(Type type, IObjectFactory factory);
        void AddObjectFactory(int typeCode, IObjectFactory factory);
        IObjectFactory GetObjectFactory(Type type);
        IObjectFactory GetObjectFactory(int typeCode);
    }

    public class PersistentContext : IPersistentContext
    {
        private Dictionary<int, IDataPersister> _dataPersisters = new Dictionary<int, IDataPersister>();
        private Dictionary<int, IObjectFactory> _objectFactories = new Dictionary<int, IObjectFactory>();
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
        public void Load(ReferenceObject dto)
        {
            // ??? if (dto.State == PersistentState.New) return;

            if (dto.State == PersistentState.Deleted || dto.State == PersistentState.Virtual)
            {
                throw new InvalidOperationException();
            }

            PersistentState oldState = dto.State;
            IDataPersister persister = this.GetDataPersister(dto.TypeCode);
            int result = persister.Select(ref dto);
            if (result == 0) throw new OptimisticConcurrencyException(dto.State.ToString());

            IPersistentStateObject pso = dto as IPersistentStateObject;
            pso.State = PersistentState.Original;

            if (oldState != dto.State)
            {
                pso.OnStateChanged(new StateEventArgs(oldState, dto.State));
            }
        }
        public void Save(ReferenceObject dto)
        {
            if (dto.State == PersistentState.Original || dto.State == PersistentState.Virtual) return;

            if (dto.State == PersistentState.Deleted) throw new InvalidOperationException();

            if (dto.State == PersistentState.New || dto.State == PersistentState.Changed)
            {
                PersistentState oldState = dto.State;
                IDataPersister persister = this.GetDataPersister(dto.TypeCode);
                if (dto.State == PersistentState.New)
                {
                    int result = persister.Insert(ref dto);
                    if (result == 0) throw new OptimisticConcurrencyException(dto.State.ToString());
                    ((IPersistentStateObject)dto).State = PersistentState.Original;
                }
                else // PersistentState.Changed
                {
                    int result = persister.Update(ref dto);
                    if (result == 0)
                    {
                        ((IPersistentStateObject)dto).State = PersistentState.Changed;
                    }
                    else if (result == 1)
                    {
                        ((IPersistentStateObject)dto).State = PersistentState.Original;
                    }
                    else if(result == -1)
                    {
                        ((IPersistentStateObject)dto).State = PersistentState.Deleted;
                    }
                    if (result != 1) throw new OptimisticConcurrencyException(dto.State.ToString());
                }
                ((IPersistentStateObject)dto).OnStateChanged(new StateEventArgs(oldState, dto.State));
            }
        }
        public void Kill(ReferenceObject dto)
        {
            if (dto.State == PersistentState.Deleted) return;

            if (dto.State == PersistentState.New) throw new InvalidOperationException();

            if (dto.State == PersistentState.Original || dto.State == PersistentState.Changed || dto.State == PersistentState.Virtual)
            {
                PersistentState oldState = dto.State;
                IDataPersister persister = this.GetDataPersister(dto.TypeCode);
                int result = persister.Delete(ref dto);
                if (result == 0) throw new OptimisticConcurrencyException(dto.State.ToString());
                IPersistentStateObject pso = dto as IPersistentStateObject;
                pso.State = PersistentState.Deleted;
                if (oldState != dto.State)
                {
                    pso.OnStateChanged(new StateEventArgs(oldState, dto.State));
                }
            }
        }
        public void AddObjectFactory(Type type, IObjectFactory factory)
        {
            TypeCodeAttribute tca = (TypeCodeAttribute)type.GetCustomAttributes(typeof(TypeCodeAttribute), false)[0];
            _objectFactories.Add(tca.TypeCode, factory);
        }
        public void AddObjectFactory(int typeCode, IObjectFactory factory)
        {
            _objectFactories.Add(typeCode, factory);
        }
        public IObjectFactory GetObjectFactory(Type type)
        {
            TypeCodeAttribute tca = (TypeCodeAttribute)type.GetCustomAttributes(typeof(TypeCodeAttribute), false)[0];
            return this.GetObjectFactory(tca.TypeCode);
        }
        public IObjectFactory GetObjectFactory(int typeCode)
        {
            return _objectFactories[typeCode];
        }
    }
}
