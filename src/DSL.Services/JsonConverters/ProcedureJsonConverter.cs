using OneCSharp.DSL.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OneCSharp.DSL.Services
{
    public sealed class ProcedureJsonConverter : JsonConverter<Procedure>
    {
        private readonly IReferenceResolver _resolver;
        private readonly ISerializationBinder _binder;
        public ProcedureJsonConverter(ISerializationBinder binder, IReferenceResolver resolver)
        {
            _binder = binder;
            _resolver = resolver;
        }
        public override void Write(Utf8JsonWriter writer, Procedure value, JsonSerializerOptions options)
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
            writer.WriteNumber("$type", _binder.GetTypeCode(typeof(Procedure)));
            writer.WriteString("Name", value.Name);
            writer.WriteNull("Parent");

            if (value.Parameters == null)
            {
                writer.WriteNull("Parameters");
            }
            else if (value.Parameters.Count == 0)
            {
                writer.WriteStartArray("Parameters");
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteStartArray("Parameters");
                foreach (Parameter parameter in value.Parameters)
                {
                    JsonSerializer.Serialize(writer, parameter, typeof(Parameter), options);
                }
                writer.WriteEndArray();
            }

            if (value.Statements == null)
            {
                writer.WriteNull("Statements");
            }
            else if (value.Statements.Count == 0)
            {
                writer.WriteStartArray("Statements");
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteStartArray("Statements");
                foreach (ISyntaxNode node in value.Statements)
                {
                    JsonSerializer.Serialize(writer, node, node.GetType(), options);
                }
                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }
        public override Procedure Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
