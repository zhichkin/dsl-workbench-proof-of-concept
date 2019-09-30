namespace OneCSharp.Persistence.Shared
{
    public abstract class ValueObject<TKey> : PersistentObject<TKey> where TKey : class
    {
        protected ValueObject()
        {
            this.StateChanged += this.ValueObject_StateChanged;
        }
        protected ValueObject(TKey key) : this() { _key = key; }

        private void ValueObject_StateChanged(IStateObject sender, StateEventArgs args)
        {
            if (args.NewState == PersistentState.Original) { this.UpdateKeyValues(); }
        }
        protected abstract void UpdateKeyValues();
    }
}
