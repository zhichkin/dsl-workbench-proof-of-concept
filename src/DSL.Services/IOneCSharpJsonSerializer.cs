using OneCSharp.DSL.Model;
using System.Text.Json;

namespace OneCSharp.DSL.Services
{
    public interface IOneCSharpJsonSerializer
    {
        Procedure FromJson(string json);
        string ToJson(Procedure procedure);
    }
    public sealed class OneCSharpJsonSerializer : IOneCSharpJsonSerializer
    {
        private readonly IReferenceResolver _resolver = new JsonReferenceResolver();
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();
        public OneCSharpJsonSerializer()
        {
            _options.WriteIndented = true;
            _options.Converters.Add(new ProcedureJsonConverter(_resolver));
        }
        public string ToJson(Procedure procedure)
        {
            _resolver.Clear();
            return JsonSerializer.Serialize(procedure, _options);
        }
        public Procedure FromJson(string json)
        {
            return new Procedure();
        }
    }
}
