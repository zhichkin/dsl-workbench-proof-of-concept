using OneCSharp.Integrator.Model;
using System.Text.Json;

namespace OneCSharp.Integrator.Services
{
    public interface IJsonContractSerializer
    {
        ISerializationBinder Binder { get; }
        DummyAssembly FromJson(string json);
        string ToJson(DummyAssembly assembly);
    }
    public sealed class JsonContractSerializer : IJsonContractSerializer
    {
        private readonly IReferenceResolver _resolver = new JsonReferenceResolver();
        private readonly ISerializationBinder _binder = new JsonSerializationBinder();
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();
        public JsonContractSerializer()
        {
            _options.WriteIndented = true;
            _options.Converters.Add(new JsonContractConverter(_binder, _resolver));
        }
        public ISerializationBinder Binder { get { return _binder; } }
        public string ToJson(DummyAssembly entity)
        {
            _resolver.Clear();
            return JsonSerializer.Serialize(entity, _options);
        }
        public DummyAssembly FromJson(string json)
        {
            return JsonSerializer.Deserialize<DummyAssembly>(json, _options);
        }
    }
}