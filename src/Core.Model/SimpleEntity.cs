namespace OneCSharp.Core
{
    public abstract class SimpleEntity : Entity
    {
        public Namespace Namespace { get; set; }
    }
    public sealed class Binary : SimpleEntity
    {
        public Binary() { Name = nameof(Binary); }
    }
    public sealed class Boolean : SimpleEntity
    {
        public Boolean() { Name = nameof(Boolean); }
    }
    public sealed class Numeric : SimpleEntity
    {
        public Numeric() { Name = nameof(Numeric); }
    }
    public sealed class String : SimpleEntity
    {
        public String() { Name = nameof(String); }
    }
    public sealed class DateTime : SimpleEntity
    {
        public DateTime() { Name = nameof(DateTime); }
    }
    public sealed class UUID : SimpleEntity
    {
        public UUID() { Name = nameof(UUID); }
    }
}