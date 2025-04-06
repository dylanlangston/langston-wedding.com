using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.SharedKernel.DomainEvents;

public class DomainEventJsonConverter : JsonConverter<DomainEvent>
{
    public override DomainEvent? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using (JsonDocument document = JsonDocument.ParseValue(ref reader))
        {
            var referenceMap = new Dictionary<int, object>();
            object? result = Deserializer.ReadObject(document.RootElement, referenceMap, typeToConvert);
            return (DomainEvent?)result;
        }
    }

    static class Deserializer
    {
        /// <summary>
        /// Recursively reads a JsonElement and reconstructs an object.
        /// </summary>
        internal static object? ReadObject(JsonElement element, Dictionary<int, object> referenceMap, Type expectedType)
        {
            if (element.ValueKind == JsonValueKind.Null)
                return null;

            // Handle simple values.
            if (IsJsonSimple(element))
            {
                return ReadSimple(element, expectedType);
            }

            // Handle arrays.
            if (element.ValueKind == JsonValueKind.Array)
            {
                // For arrays, assume a List of the expected element type.
                Type itemType = expectedType.IsArray ? expectedType.GetElementType()! : typeof(object);
                var listType = typeof(List<>).MakeGenericType(itemType);
                var list = (IList)Activator.CreateInstance(listType)!;
                foreach (var item in element.EnumerateArray())
                {
                    list.Add(ReadObject(item, referenceMap, itemType));
                }
                if (expectedType.IsArray)
                {
                    Array array = Array.CreateInstance(itemType, list.Count);
                    list.CopyTo(array, 0);
                    return array;
                }
                return list;
            }

            // At this point, we assume the element is an object.
            // Check if this is a reference.
            if (element.TryGetProperty("$ref", out JsonElement refElement))
            {
                int refId = refElement.GetInt32();
                return referenceMap[refId];
            }

            // Otherwise, create a new instance.
            // Retrieve type information.
            string typeName = expectedType.AssemblyQualifiedName!;
            if (element.TryGetProperty("$type", out JsonElement typeElement))
            {
                typeName = typeElement.GetString()!;
            }
            Type type = Type.GetType(typeName) ?? expectedType;

            // Create an uninitialized object.
            object obj = RuntimeHelpers.GetUninitializedObject(type);

            // If the object has an assigned id, record it.
            if (element.TryGetProperty("$id", out JsonElement idElement))
            {
                int id = idElement.GetInt32();
                referenceMap[id] = obj;
            }

            // Process each property in the JSON (skip the metadata: $id, $type).
            foreach (JsonProperty property in element.EnumerateObject())
            {
                if (property.NameEquals("$id") || property.NameEquals("$type"))
                    continue;

                // Try to find a matching property (by name) in the type.
                PropertyInfo propInfo = type.GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;
                if (propInfo != null && propInfo.CanWrite)
                {
                    object? value = ReadObject(property.Value, referenceMap, propInfo.PropertyType);
                    propInfo.SetValue(obj, value);
                    continue;
                }

                // Otherwise, try to set a field.
                FieldInfo fieldInfo = type.GetField(property.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;
                if (fieldInfo != null)
                {
                    object? value = ReadObject(property.Value, referenceMap, fieldInfo.FieldType);
                    fieldInfo.SetValue(obj, value);
                }
            }

            return obj;
        }

        /// <summary>
        /// Returns true if the JsonElement represents a simple value.
        /// </summary>
        private static bool IsJsonSimple(JsonElement element)
        {
            return element.ValueKind == JsonValueKind.String ||
                   element.ValueKind == JsonValueKind.Number ||
                   element.ValueKind == JsonValueKind.True ||
                   element.ValueKind == JsonValueKind.False;
        }

        private static readonly Dictionary<Type, Func<JsonElement, object?>> SimpleTypeReaders = new Dictionary<Type, Func<JsonElement, object?>>
        {
            { typeof(string), (element) => element.GetString() },
            { typeof(bool), (element) =>  element.GetBoolean() },
            { typeof(int), (element) =>  element.GetInt32() },
            { typeof(long), (element) => element.GetInt64() },
            { typeof(float), (element) => element.GetSingle() },
            { typeof(double), (element) => element.GetDouble() },
            { typeof(decimal), (element) => element.GetDecimal() },
            { typeof(DateTime), (element) => element.GetDateTime() },
            { typeof(DateTimeOffset), (element) => element.GetDateTimeOffset() },
            { typeof(Guid), (element) => element.GetGuid() }
        };

        /// <summary>
        /// Reads a simple value from a JsonElement.
        /// </summary>
        private static object? ReadSimple(JsonElement element, Type expectedType)
        {
            if (SimpleTypeReaders.TryGetValue(expectedType, out var simpleReader))
            {

                return simpleReader(element);
            }

            // Fallback to string.
            return element.GetString();
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        DomainEvent domainEvent,
        JsonSerializerOptions options)
    {
        int nextId = 1;
        // visited: object -> assigned id
        var visited = new Dictionary<object, int>(ReferenceEqualityComparer.Instance);
        Serializer.WriteObject(domainEvent, writer, visited, ref nextId);
    }

    static class Serializer
    {
        /// <summary>
        /// Recursively writes an object to the Utf8JsonWriter.
        /// </summary>
        internal static void WriteObject(object? obj, Utf8JsonWriter writer, Dictionary<object, int> visited, ref int nextId)
        {
            if (obj == null)
            {
                writer.WriteNullValue();
                return;
            }

            Type type = obj.GetType();

            // Simple types are directly written.
            if (IsSimple(type))
            {
                WriteSimple(obj, writer, type);
                return;
            }

            // Handle collections (except string, which is simple)
            if (obj is IEnumerable enumerable && !(obj is string))
            {
                writer.WriteStartArray();
                foreach (var item in enumerable)
                {
                    WriteObject(item, writer, visited, ref nextId);
                }
                writer.WriteEndArray();
                return;
            }

            // Handle cyclic references:
            if (visited.TryGetValue(obj, out int existingId))
            {
                // Write a reference object.
                writer.WriteStartObject();
                writer.WriteNumber(JsonEncodedText.Encode("$ref"), existingId);
                writer.WriteEndObject();
                return;
            }
            // Assign new id and record object.
            int currentId = nextId++;
            visited[obj] = currentId;

            // Write object with metadata.
            writer.WriteStartObject();
            writer.WriteNumber(JsonEncodedText.Encode("$id"), currentId);
            writer.WriteString(JsonEncodedText.Encode("$type"), type.AssemblyQualifiedName);

            // Write fields.
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                writer.WritePropertyName(JsonEncodedText.Encode(field.Name));
                object? fieldValue = field.GetValue(obj);
                WriteObject(fieldValue, writer, visited, ref nextId);
            }

            // Write properties (exclude indexers and those that can’t be read).
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                 .Where(p => p.GetIndexParameters().Length == 0 && p.CanRead);
            foreach (var prop in properties)
            {
                writer.WritePropertyName(JsonEncodedText.Encode(prop.Name));
                object? propValue;
                try
                {
                    propValue = prop.GetValue(obj);
                }
                catch
                {
                    propValue = null;
                }
                WriteObject(propValue, writer, visited, ref nextId);
            }
            writer.WriteEndObject();
        }

        private static readonly Dictionary<Type, Action<object, Utf8JsonWriter>> SimpleTypeWriters = new Dictionary<Type, Action<object, Utf8JsonWriter>>
        {
            { typeof(string), (obj, writer) => writer.WriteStringValue((string)obj) },
            { typeof(char), (obj, writer) => writer.WriteStringValue(((char)obj).ToString()) },
            { typeof(bool), (obj, writer) => writer.WriteBooleanValue((bool)obj) },
            { typeof(int), (obj, writer) => writer.WriteNumberValue((int)obj) },
            { typeof(long), (obj, writer) => writer.WriteNumberValue((long)obj) },
            { typeof(float), (obj, writer) => writer.WriteNumberValue((float)obj) },
            { typeof(double), (obj, writer) => writer.WriteNumberValue((double)obj) },
            { typeof(decimal), (obj, writer) => writer.WriteNumberValue((decimal)obj) },
            { typeof(DateTime), (obj, writer) => writer.WriteStringValue(((DateTime)obj).ToString("o")) },
            { typeof(DateTimeOffset), (obj, writer) => writer.WriteStringValue(((DateTimeOffset)obj).ToString("o")) },
            { typeof(Guid), (obj, writer) => writer.WriteStringValue(((Guid)obj).ToString()) }
        };

        private static void WriteSimple(object obj, Utf8JsonWriter writer, Type type)
        {
            if (obj == null)
            {
                writer.WriteNullValue();
                return;
            }

            if (SimpleTypeWriters.TryGetValue(type, out var simpleWriter))
            {
                simpleWriter(obj, writer);
                return;
            }

            // Fallback: write as string.
            writer.WriteStringValue(obj.ToString());
        }

        /// <summary>
        /// Returns true if the type is considered a simple (primitive or value) type.
        /// </summary>
        private static bool IsSimple(Type type)
        {
            return type.IsPrimitive ||
                   SimpleTypeWriters.ContainsKey(type) ||
                   Convert.GetTypeCode(type) != TypeCode.Object;
        }
    }
}