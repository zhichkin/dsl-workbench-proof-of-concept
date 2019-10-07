using System;

namespace OneCSharp.Persistence.Shared
{
    public interface IPersistentObject
    {
        int TypeCode { get; }
    }
    public interface IPersistentObject<TKey> : IPersistentObject
    {
        TKey PrimaryKey { get; }
    }

    public abstract class PersistentObject<TKey> : StateObject, IPersistentObject<TKey>
    {
        public PersistentObject() { }
        public PersistentObject(TKey primaryKey) { _key = primaryKey; }
        public abstract int TypeCode { get; }

        protected TKey _key;
        public TKey PrimaryKey { get { return _key; } }

        public abstract class PrimaryKeyInsider
        {
            protected void SetPrimaryKey(PersistentObject<TKey> target, TKey key)
            {
                target._key = key;
            }
        }
    }
}
