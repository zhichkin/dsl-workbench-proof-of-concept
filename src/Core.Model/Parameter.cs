namespace OneCSharp.Core
{
    public class Parameter : Entity
    {
        public Method Owner { get; set; }
        public Entity ArgumentType { get; set; }
        public object DefaultValue { get; set; }
    }
}