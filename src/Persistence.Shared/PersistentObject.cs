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

        protected int _typeCode;
        public int TypeCode { get { return _typeCode; } }

        protected TKey _key;
        public TKey PrimaryKey { get { return _key; } }
    }
}
