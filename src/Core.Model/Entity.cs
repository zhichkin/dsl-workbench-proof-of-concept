using System.Collections;

namespace OneCSharp.Core
{
    public interface IEntity
    {
        string Name { get; set; }
    }
    public abstract class Entity : IEntity
    {
        public string Name { get; set; }
    }
    public interface IHaveChildren
    {
        void AddChild(Entity child);
        IEnumerable Children { get; }
    }
}