namespace OneCSharp.Persistence.Shared
{
    public abstract class ReferenceObject<TKey> : PersistentObject<TKey> where TKey : struct
    {
        protected ReferenceObject() { }
        protected ReferenceObject(TKey key) { _key = key; }
        public ObjectReference<TKey> GetReference()
        {
            return new ObjectReference<TKey>(this);
        }
    }
}
