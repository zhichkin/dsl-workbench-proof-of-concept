namespace OneCSharp.Core
{
    public interface IEntity
    {
        string Name { get; set; }
    }
    public abstract class Entity : IEntity
    {
        [PropertyPurpose(PropertyPurpose.Presentation)] public string Name { get; set; }
    }
}