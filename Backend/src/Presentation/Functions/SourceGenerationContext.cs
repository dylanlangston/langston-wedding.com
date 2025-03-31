using Functions.DTOs;

namespace Functions;

/// <summary>
/// Source Based serializer for the Function App DTOs
/// </summary>
[JsonSourceGenerationOptions()]
[JsonSerializable(typeof(ContactRequest))]
[JsonSerializable(typeof(MessageResponse))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}