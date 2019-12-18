using OneCSharp.DSL.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OneCSharp.DSL.Services
{
    public sealed class ParameterJsonConverter : JsonConverter<Parameter>
    {
        private readonly IReferenceResolver _resolver;
        private readonly ISerializationBinder _binder;
        public ParameterJsonConverter(ISerializationBinder binder, IReferenceResolver resolver)
        {
            _binder = binder;
            _resolver = resolver;
        }
        public override void Write(Utf8JsonWriter writer, Parameter value, JsonSerializerOptions options)
        {
            bool isNew = false;
            string id = _resolver.GetReference(value, ref isNew);

            if (!isNew)
            {
                writer.WriteStartObject();
                writer.WriteString("$ref", id);
                writer.WriteEndObject();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString("$id", id);
            writer.WriteNumber("$type", _binder.GetTypeCode(typeof(Parameter)));
            writer.WriteString("Name", value.Name);
            if (value.Parent == null)
            {
                writer.WriteNull("Parent");
            }
            else
            {
                writer.WritePropertyName("Parent");
                JsonSerializer.Serialize(writer, value.Parent, value.Parent.GetType(), options);
            }
            writer.WriteString("Type", value.Type.Name);
            writer.WriteBoolean("IsInput", value.IsInput);
            writer.WriteBoolean("IsOutput", value.IsOutput);
            writer.WriteEndObject();
        }
        public override Parameter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
