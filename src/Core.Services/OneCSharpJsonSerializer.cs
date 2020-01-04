using OneCSharp.Core.Model;
using System.Text.Json;

namespace OneCSharp.Core.Services
{
    public interface IOneCSharpJsonSerializer
    {
        ISerializationBinder Binder { get; }
        Entity FromJson(string json);
        string ToJson(Entity entity);
    }
    public sealed class OneCSharpJsonSerializer : IOneCSharpJsonSerializer
    {
        private readonly IReferenceResolver _resolver = new JsonReferenceResolver();
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();
        private readonly ISerializationBinder _binder = new JsonSerializationBinder();
        public OneCSharpJsonSerializer()
        {
            _options.WriteIndented = true;
            _options.Converters.Add(new EntityJsonConverter(_binder, _resolver));
        }
        public ISerializationBinder Binder { get { return _binder; } }
        public string ToJson(Entity entity)
        {
            _resolver.Clear();
            return JsonSerializer.Serialize(entity, _options);
        }
        public Entity FromJson(string json)
        {
            return JsonSerializer.Deserialize<Entity>(json, _options);
        }
    }
}