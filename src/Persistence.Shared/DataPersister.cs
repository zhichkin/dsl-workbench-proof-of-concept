using System;

namespace OneCSharp.Persistence.Shared
{
    public interface IDataPersister
    {
        void Select(IPersistentObject persistentObject);
        void Insert(IPersistentObject persistentObject);
        void Update(IPersistentObject persistentObject);
        void Delete(IPersistentObject persistentObject);
    }

    public abstract class DataPersister<TPersistent> : StateObject.Insider where TPersistent : StateObject, IPersistentObject
    {
        protected abstract void Select(TPersistent persistentObject);
        protected abstract void Insert(TPersistent persistentObject);
        protected abstract void Update(TPersistent persistentObject);
        protected abstract void Delete(TPersistent persistentObject);

        public void Load(TPersistent persistentObject)
        {
            if (persistentObject.State == PersistentState.Changed || persistentObject.State == PersistentState.Original)
            {
                this.Select(persistentObject);
                base.SetState(persistentObject, PersistentState.Original);
            }
        }
        public void Save(TPersistent persistentObject)
        {
            if (persistentObject.State == PersistentState.New || persistentObject.State == PersistentState.Changed)
            {
                if (persistentObject.State == PersistentState.New)
                {
                    this.Insert(persistentObject);
                }
                else
                {
                    this.Update(persistentObject);
                }
                base.SetState(persistentObject, PersistentState.Original);
            }
        }
        public void Kill(TPersistent persistentObject)
        {
            if (persistentObject.State == PersistentState.Original || persistentObject.State == PersistentState.Changed)
            {
                this.Delete(persistentObject);
                base.SetState(persistentObject, PersistentState.Deleted);
            }
        }
    }
}
