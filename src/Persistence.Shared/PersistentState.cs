using System;

namespace OneCSharp.Persistence.Shared
{
    public enum PersistentState : byte
    {
        New,      // объект только что создан в памяти, ещё не существует в источнике данных
        Original, // объект загружен из источника данных и ещё ни разу с тех пор не изменялся
        Changed,  // объект загружен из источника данных и с тех пор был уже изменен
        Deleted,  // объект удалён из источника данных, но пока ещё существует в памяти
        Virtual   // ссылка на объект в базе данных { TypeCode + PrimaryKey + Presentation }
    }
    public interface IPersistentStateObject
    {
        PersistentState State { get; set; }
        void OnStateChanged(StateEventArgs args);
        event StateChangedEventHandler StateChanged;
    }
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
    public delegate void StateChangedEventHandler(IPersistentStateObject sender, StateEventArgs args);
}
