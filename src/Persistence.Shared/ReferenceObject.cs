using System;
using System.Collections.Generic;
using System.Text;

namespace OneCSharp.Persistence.Shared
{
    public interface IDataTransferObject
    {
        int TypeCode { get; set; }
    }

    public interface IPersistentObject<TKey> : IDataTransferObject
    {
        TKey PrimaryKey { get; set; }
    }
    public interface IReferenceObject<TKey> : IPersistentObject<TKey> where TKey : struct
    {
        string Presentation { get; set; }
    }
    public interface IValueObject<TKey> : IPersistentObject<TKey> where TKey : class { }
    public class ReferenceObject : IReferenceObject<Guid>, IPersistentStateObject
    {
        protected int _typeCode = 0;
        protected Guid _primaryKey = Guid.Empty;
        protected string _presentation = string.Empty;
        protected PersistentState _state = PersistentState.Virtual;
        public event StateChangedEventHandler StateChanged;
        protected ReferenceObject() { }
        public ReferenceObject(int typeCode, Guid primaryKey, string presentation)
        {
            _typeCode = typeCode;
            _primaryKey = primaryKey;
            _presentation = presentation;
        }
        public int TypeCode { get { return _typeCode; } private set { _typeCode = value; } }
        int IDataTransferObject.TypeCode { get { return this.TypeCode; } set { this.TypeCode = value; } }
        public Guid PrimaryKey { get { return _primaryKey; } }
        Guid IPersistentObject<Guid>.PrimaryKey { get { return _primaryKey; } set { _primaryKey = value; } }
        string IReferenceObject<Guid>.Presentation { get { return _presentation; } set { _presentation = value; } }
        public PersistentState State { get { return _state; } }
        PersistentState IPersistentStateObject.State { get { return _state; } set { _state = value; } }
        public bool IsEmpty() { return (this.PrimaryKey == Guid.Empty); }
        void IPersistentStateObject.OnStateChanged(StateEventArgs args)
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
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_presentation))
            {
                return $"{{{this.TypeCode.ToString()}:{this.PrimaryKey.ToString()}}}";
            }
            return _presentation;
        }
        public override int GetHashCode() { return this.PrimaryKey.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            ReferenceObject test = obj as ReferenceObject;
            if (test == null) { return false; }
            return (this.TypeCode == test.TypeCode && this.PrimaryKey == test.PrimaryKey);
        }
        public static bool operator ==(ReferenceObject left, ReferenceObject right)
        {
            if (object.ReferenceEquals(left, right)) { return true; }
            if (((object)left == null) || ((object)right == null)) { return false; }
            return left.Equals(right);
        }
        public static bool operator !=(ReferenceObject left, ReferenceObject right)
        {
            return !(left == right);
        }
    }
}
