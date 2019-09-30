namespace OneCSharp.Persistence.Shared
{
    public interface IObjectReference<TKey> : IPersistentObject<TKey> where TKey : struct
    {
        string Presentation { get; }
    }

    public sealed class ObjectReference<TKey> : IObjectReference<TKey> where TKey : struct
    {
        public ObjectReference(int typeCode, TKey primaryKey, string presentation)
        {
            this.TypeCode = typeCode;
            this.PrimaryKey = primaryKey;
            this.Presentation = presentation;
        }
        public ObjectReference(ReferenceObject<TKey> reference) : this(
                  reference.TypeCode,
                  reference.PrimaryKey,
                  reference.ToString()) { }

        public int TypeCode { get; private set; }
        public TKey PrimaryKey { get; private set; }
        public string Presentation { get; private set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Presentation))
            {
                return $"{{{this.TypeCode.ToString()}:{this.PrimaryKey.ToString()}}}";
            }
            return this.Presentation;
        }
        public override int GetHashCode() { return this.PrimaryKey.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            ObjectReference<TKey> test = obj as ObjectReference<TKey>;
            if (test == null) { return false; }
            return this.TypeCode == test.TypeCode && this.PrimaryKey.Equals(test.PrimaryKey);
        }
        public static bool operator ==(ObjectReference<TKey> left, ObjectReference<TKey> right)
        {
            if (object.ReferenceEquals(left, right)) { return true; }
            if (((object)left == null) || ((object)right == null)) { return false; }
            return left.Equals(right);
        }
        public static bool operator !=(ObjectReference<TKey> left, ObjectReference<TKey> right)
        {
            return !(left == right);
        }
    }
}
