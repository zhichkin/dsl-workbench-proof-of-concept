using System;
using System.Collections.Generic;
using System.Text;

namespace OneCSharp.Persistence.Shared
{
    public class StateObject : IPersistentState
    {
        public event StateChangedEventHandler StateChanged;
        protected PersistentState _state = PersistentState.New;
        public StateObject() { }
        PersistentState IPersistentState.State { get { return _state; } set { _state = value; } }
        void IPersistentState.OnStateChanged(StateEventArgs args)
        {
            StateChanged?.Invoke(this, args);
        }
        protected void Set<TValue>(TValue value, ref TValue storage)
        {
            if (_state == PersistentState.Deleted) return;

            if (_state == PersistentState.New || _state == PersistentState.Changed || _state == PersistentState.Virtual)
            {
                storage = value;
                return;
            }

            bool changed = (storage != null)
                ? !storage.Equals(value)
                : (value != null);

            if (changed)
            {
                StateEventArgs args = new StateEventArgs(PersistentState.Original, PersistentState.Changed);
                storage = value;
                _state = PersistentState.Changed;
                StateChanged?.Invoke(this, args);
            }
        }
    }
}
