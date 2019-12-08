using OneCSharp.DSL.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OneCSharp.DSL.Services
{
    public sealed class ProcedureJsonConverter : JsonConverter<Procedure>
    {
        private readonly IReferenceResolver _resolver;
        public ProcedureJsonConverter(IReferenceResolver resolver)
        {
            _resolver = resolver;
        }
        public override void Write(Utf8JsonWriter writer, Procedure value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("$id", new Guid(_resolver.GetReference(value)));
            writer.WriteString("$type", nameof(Procedure));
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
                    writer.WriteStringValue(JsonSerializer.Serialize(parameter, typeof(Parameter), options));
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
                    writer.WriteStringValue(JsonSerializer.Serialize(node, node.GetType(), options));
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
