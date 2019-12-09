using OneCSharp.DSL.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OneCSharp.DSL.Services
{
    public sealed class SelectStatementJsonConverter : JsonConverter<SelectStatement>
    {
        private readonly IReferenceResolver _resolver;
        private readonly ISerializationBinder _binder;
        public SelectStatementJsonConverter(ISerializationBinder binder, IReferenceResolver resolver)
        {
            _binder = binder;
            _resolver = resolver;
        }
        public override void Write(Utf8JsonWriter writer, SelectStatement value, JsonSerializerOptions options)
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
            writer.WriteNumber("$type", _binder.GetTypeCode(typeof(SelectStatement)));
            if (value.Parent == null)
            {
                writer.WriteNull("Parent");
            }
            else
            {
                writer.WritePropertyName("Parent");
                JsonSerializer.Serialize(writer, value.Parent, value.Parent.GetType(), options);
            }
            writer.WriteString("Alias", value.Alias);
            if (value.SELECT == null)
            {
                writer.WriteNull("SELECT");
            }
            else
            {
                writer.WritePropertyName("SELECT");
                SyntaxNodeListJsonConverter.Write(writer, value.SELECT, options);
            }
            if (value.FROM == null)
            {
                writer.WriteNull("FROM");
            }
            else
            {
                writer.WritePropertyName("FROM");
                SyntaxNodeListJsonConverter.Write(writer, value.FROM, options);
            }
            if (value.WHERE == null)
            {
                writer.WriteNull("WHERE");
            }
            else
            {
                writer.WritePropertyName("WHERE");
                JsonSerializer.Serialize(writer, value.WHERE, value.WHERE.GetType(), options);
            }
            writer.WriteEndObject();
        }
        public override SelectStatement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

