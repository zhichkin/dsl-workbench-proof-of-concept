using OneCSharp.Core;

namespace OneCSharp.SQL.Model
{
    public sealed class TypeInfo
    {
        public int TypeCode { get; set; }
        public string Name { get; set; }
        public string UUID { get; set; }
        public Entity Entity { get; set; }
    }
}