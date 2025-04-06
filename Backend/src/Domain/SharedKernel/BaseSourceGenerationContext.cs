using System.Text.Json.Serialization;
using Domain.Contact.Aggregates;
using Domain.SharedKernel.DomainEvents;

namespace Domain.SharedKernel;

/// <summary>
/// Source Based serializer for the Function App DTOs
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(ContactRequest))]
[JsonSerializable(typeof(DomainEvent))]
public partial class BaseSourceGenerationContext : JsonSerializerContext
{
}

