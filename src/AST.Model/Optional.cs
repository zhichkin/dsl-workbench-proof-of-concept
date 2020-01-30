namespace OneCSharp.AST.Model
{
    public interface IOptional
    {
        object Value { get; set; }
        bool HasValue { get; set; }
    }
    public sealed class Optional<T> : IOptional
    {
        private T _value = default;
        public Optional() { }
        public Optional(T value) { Value = value; }
        public bool HasValue { get; set; } = false;
        object IOptional.Value { get { return _value; } set { Value = (T)value; } }
        public T Value { get { return _value; } set { _value = value; HasValue = true; } }
    }
}