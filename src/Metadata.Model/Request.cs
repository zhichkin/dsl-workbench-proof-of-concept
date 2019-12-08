namespace OneCSharp.Metadata.Model
{
    public interface IRequest
    {
        public INamespace Parent { get; set; }
        public string Name { get; set; }
        // TODO: parameters collection
    }
    public sealed class Request : IRequest
    {
        public INamespace Parent { get; set; }
        public string Name { get; set; }
        // TODO: parameters collection
    }
}
