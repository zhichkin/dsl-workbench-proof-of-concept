namespace OneCSharp.Core
{
    public class Parameter : Entity
    {
        [PropertyPurpose(PropertyPurpose.Hierarchy)] public Method Owner { get; set; }
        public Entity ArgumentType { get; set; }
        public object DefaultValue { get; set; }
    }
}