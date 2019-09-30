using System;

namespace OneCSharp.Persistence.Shared
{
    public class StateEventArgs : EventArgs
    {
        private StateEventArgs() { }
        public StateEventArgs(PersistentState oldState, PersistentState newState)
        {
            this.OldState = oldState;
            this.NewState = newState;
        }
        public PersistentState OldState { get; }
        public PersistentState NewState { get; }
    }
    public delegate void StateChangedEventHandler(IStateObject sender, StateEventArgs args);
    public delegate void StateChangingEventHandler(IStateObject sender, StateEventArgs args);

    public interface IStateObject
    {
        PersistentState State { get; }
        event StateChangedEventHandler StateChanged;
        event StateChangingEventHandler StateChanging;
    }

    public abstract class StateObject : IStateObject
    {
        public StateObject() { }

        protected PersistentState _state = PersistentState.New;
        public PersistentState State { get; }

        public event StateChangedEventHandler StateChanged;
        public event StateChangingEventHandler StateChanging;
        protected void OnStateChanged(StateEventArgs args)
        {
            StateChanged?.Invoke(this, args);
        }
        protected void OnStateChanging(StateEventArgs args)
        {
            StateChanging?.Invoke(this, args);
        }

        protected TValue Get<TValue>(ref TValue storage)
        {
            return storage;
        }
        protected void Set<TValue>(TValue value, ref TValue storage)
        {
            if (_state == PersistentState.Virtual) throw new InvalidOperationException();

            if (_state == PersistentState.Deleted) return;

            if (_state == PersistentState.New || _state == PersistentState.Changed)
            {
                storage = value;
                return;
            }

            // _state == PersistentState.Original

            bool changed = (storage != null)
                ? !storage.Equals(value)
                : (value != null);

            if (changed)
            {
                StateEventArgs args = new StateEventArgs(PersistentState.Original, PersistentState.Changed);
                OnStateChanging(args);
                storage = value;
                _state = PersistentState.Changed;
                OnStateChanged(args);
            }
        }

        public abstract class Insider
        {
            protected void SetState(StateObject target, PersistentState state)
            {
                if (target._state == state) return;

                StateEventArgs args = new StateEventArgs(target._state, state);
                target.OnStateChanging(args);
                target._state = state;
                target.OnStateChanged(args);
            }
        }
    }
}
