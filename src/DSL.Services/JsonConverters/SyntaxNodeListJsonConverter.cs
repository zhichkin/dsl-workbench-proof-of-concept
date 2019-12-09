using OneCSharp.DSL.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OneCSharp.DSL.Services
{
    public static class SyntaxNodeListJsonConverter
    {
        private static readonly ISerializationBinder _binder = new JsonSerializationBinder();
        public static void Write(Utf8JsonWriter writer, SyntaxNodeList value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("$id", Guid.NewGuid().ToString());
            writer.WriteNumber("$type", _binder.GetTypeCode(value.GetType()));
            if (value.Parent == null)
            {
                writer.WriteNull("Parent");
            }
            else
            {
                writer.WritePropertyName("Parent");
                JsonSerializer.Serialize(writer, value.Parent, value.Parent.GetType(), options);
            }
            if (value.Count == 0)
            {
                writer.WriteStartArray("Nodes");
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteStartArray("Nodes");
                foreach (ISyntaxNode node in value)
                {
                    JsonSerializer.Serialize(writer, node, node.GetType(), options);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndObject();
        }
        public static SyntaxNodeList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}