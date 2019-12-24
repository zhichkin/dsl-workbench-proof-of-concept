using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OneCSharp.Core.Services
{
    public sealed class EntityJsonConverter : JsonConverter<Entity>
    {
        private readonly IReferenceResolver _resolver;
        private readonly ISerializationBinder _binder;
        public EntityJsonConverter(ISerializationBinder binder, IReferenceResolver resolver) : base()
        {
            _binder = binder;
            _resolver = resolver;
        }
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert == null) return false;
            return typeToConvert.IsAssignableFrom(typeof(Entity));
        }
        public override void Write(Utf8JsonWriter writer, Entity value, JsonSerializerOptions options)
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

            Type type = value.GetType();
            writer.WriteStartObject();
            writer.WriteString("$id", id);
            writer.WriteNumber("$type", _binder.GetTypeCode(type));
            foreach (PropertyInfo info in type.GetProperties())
            {
                WriteProperty(writer, value, info, options);
            }
            writer.WriteEndObject();
        }
        private void WriteProperty(Utf8JsonWriter writer, Entity source, PropertyInfo info, JsonSerializerOptions options)
        {
            if (info.PropertyType == typeof(IEnumerable))
            {
                return; // IEnumerable IHierarchy.Children
            }

            writer.WritePropertyName(info.Name);
            object value = info.GetValue(source);

            if (info.PropertyType.IsGenericType) // we are interested in List<T>
            {
                WriteArray(writer, (IEnumerable)value, options);
            }
            else if (value is Entity)
            {
                Write(writer, (Entity)value, options);
            }
            else
            {
                JsonSerializer.Serialize(writer, value, info.PropertyType);
            }
        }
        private void WriteArray(Utf8JsonWriter writer, IEnumerable list, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            IEnumerator enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is Entity entity)
                {
                    Write(writer, entity, options);
                }
                else
                {
                    JsonSerializer.Serialize(writer, enumerator.Current, enumerator.Current.GetType());
                }
            }
            writer.WriteEndArray();
        }



        public override Entity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ReadObject(ref reader, options);
        }
        private Entity ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            byte[] ID = Encoding.UTF8.GetBytes("$id");
            byte[] REF = Encoding.UTF8.GetBytes("$ref");
            byte[] TYPE = Encoding.UTF8.GetBytes("$type");
            
            string reference1 = string.Empty;
            string propertyName = string.Empty;
            PropertyInfo propertyInfo = null;

            Entity entity = null;
            Type entityType = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Entity value = ReadObject(ref reader, options);
                    propertyInfo.SetValue(entity, value);
                }
                else if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return entity;
                }
                else if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (reader.ValueTextEquals(REF))
                    {
                        reader.Read();
                        string referenceN = reader.GetString();
                        entity = (Entity)_resolver.ResolveReference(referenceN);
                        while (reader.TokenType != JsonTokenType.EndObject)
                        {
                            if (!reader.Read()) { break; }
                        }
                        return entity;
                    }
                    else if (reader.ValueTextEquals(ID))
                    {
                        reader.Read();
                        reference1 = reader.GetString();
                    }
                    else if (reader.ValueTextEquals(TYPE))
                    {
                        reader.Read();
                        int typeCode = reader.GetInt32();
                        entityType = _binder.GetType(typeCode);
                        entity = (Entity)Activator.CreateInstance(entityType);
                        _resolver.AddReference(reference1, entity);
                    }
                    else
                    {
                        propertyName = reader.GetString();
                        propertyInfo = entityType.GetProperty(propertyName);
                    }
                }
                else if (reader.TokenType == JsonTokenType.Null)
                {
                    propertyInfo.SetValue(entity, null);
                }
                else if (reader.TokenType == JsonTokenType.True)
                {
                    propertyInfo.SetValue(entity, true);
                }
                else if (reader.TokenType == JsonTokenType.False)
                {
                    propertyInfo.SetValue(entity, false);
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    propertyInfo.SetValue(entity, reader.GetUInt32());
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    propertyInfo.SetValue(entity, reader.GetString());
                }
                else if (reader.TokenType == JsonTokenType.StartArray)
                {
                    bool hasChildren = (entityType.GetInterfaces()
                        .Where(i => i == typeof(IHierarchy))
                        .FirstOrDefault() != null);

                    IList list = (IList)propertyInfo.GetValue(entity);
                    while (reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (!reader.Read() || reader.TokenType == JsonTokenType.EndArray)
                        {
                            break;
                        }
                        Entity item = ReadObject(ref reader, options);
                        if (hasChildren)
                        {
                            ((IHierarchy)entity).AddChild(item);
                        }
                        else
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return entity;
        }
    }
}