using OneCSharp.AST.Model;
using System.Text.Json;

namespace OneCSharp.AST.Services
{
    public interface ISyntaxTreeJsonSerializer
    {
        ISerializationBinder Binder { get; }
        SyntaxNode FromJson(string json);
        string ToJson(SyntaxNode entity);
    }
    public sealed class SyntaxTreeJsonSerializer : ISyntaxTreeJsonSerializer
    {
        private readonly IReferenceResolver _resolver = new JsonReferenceResolver();
        private readonly ISerializationBinder _binder = new JsonSerializationBinder();
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();
        public SyntaxTreeJsonSerializer()
        {
            _options.WriteIndented = true;
            _options.Converters.Add(new SyntaxNodeJsonConverter(_binder, _resolver));
        }
        public ISerializationBinder Binder { get { return _binder; } }
        public string ToJson(SyntaxNode entity)
        {
            _resolver.Clear();
            return JsonSerializer.Serialize(entity, _options);
        }
        public SyntaxNode FromJson(string json)
        {
            return JsonSerializer.Deserialize<SyntaxNode>(json, _options);
        }
    }
}

//public Module()
//{
//    _serializer = new OneCSharpJsonSerializer();
//    var knownTypes = _serializer.Binder.KnownTypes;
//    knownTypes.Add(1, typeof(Language));
//    knownTypes.Add(2, typeof(Namespace));
//}
//public void Persist(Entity model)
//{
//    string json = _serializer.ToJson(model);

//    string filePath = GetModuleFilePath();
//    using (StreamWriter writer = File.CreateText(filePath))
//    {
//        writer.Write(json);
//    }
//}
//private void ReadModuleFromFile()
//{
//    string filePath = GetModuleFilePath();
//    string json = File.ReadAllText(filePath);
//    if (string.IsNullOrWhiteSpace(json)) return;

//    Entity entity = _serializer.FromJson(json);
//    BuildTreeView(entity);
//}