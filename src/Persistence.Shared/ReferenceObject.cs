using System;

namespace OneCSharp.Persistence.Shared
{
    public abstract class ReferenceObject<TKey> : PersistentObject<TKey> where TKey : struct
    {
        protected ReferenceObject() { }
        protected ReferenceObject(TKey key) { _key = key; }
    }

    public abstract class ReferenceObject : ReferenceObject<Guid>
    {
        protected ReferenceObject() { }
        protected ReferenceObject(Guid key) { _key = key; }
        public ObjectReference GetReference()
        {
            return new ObjectReference(this);
        }
    }

    public abstract class IdentityObject : ReferenceObject<int>
    {
        protected IdentityObject() { }
        protected IdentityObject(int key) { _key = key; }
        public IdentityReference GetReference()
        {
            return new IdentityReference(this);
        }
    }
}
