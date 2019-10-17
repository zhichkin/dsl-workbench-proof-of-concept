using System;
using System.Collections.Generic;
using System.Text;

namespace OneCSharp.Persistence.Shared
{
    public interface IDataTransferObject
    {
        int TypeCode { get; set; }
    }
    public interface IPersistentObject<TPrimaryKey> : IDataTransferObject
    {
        TPrimaryKey PrimaryKey { get; set; }
    }
    public class ReferenceObject : IPersistentObject<Guid>
    {
        private int _typeCode = 0;
        private Guid _primaryKey = Guid.Empty;
        public ReferenceObject(int typeCode, Guid primaryKey)
        {
            _typeCode = typeCode;
            _primaryKey = primaryKey;
        }
        int IDataTransferObject.TypeCode { get { return _typeCode; } set { _typeCode = value; } }
        public Guid PrimaryKey { get { return _primaryKey; } }
        Guid IPersistentObject<Guid>.PrimaryKey { get { return _primaryKey; } set { _primaryKey = value; } }
        public override string ToString()
        {
            return $"{{{_typeCode.ToString()}:{_primaryKey.ToString()}}}";
        }
        public bool IsEmpty() { return (_primaryKey == Guid.Empty); }
        public ReferenceObject GetReference() { return this; }
        public override int GetHashCode() { return _primaryKey.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            ReferenceObject test = obj as ReferenceObject;
            if (test == null) { return false; }
            return (_typeCode == test._typeCode && _primaryKey == test._primaryKey);
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
    public class ClientReferenceObject : ReferenceObject
    {
        private readonly string _view = string.Empty;
        public ClientReferenceObject(int typeCode, Guid primaryKey, string view) : base(typeCode, primaryKey)
        {
            _view = view;
        }
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_view))
            {
                return base.ToString();
            }
            return _view;
        }
    }
}
