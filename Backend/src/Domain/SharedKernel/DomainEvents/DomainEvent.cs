using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.SharedKernel.DomainEvents;

public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
    string Description { get; }

    void EnsureDescription();
}

[JsonConverter(typeof(DomainEventJsonConverter))]
public class DomainEvent : BaseEntity<DateTimeOffset>, IDomainEvent
{
    public DomainEvent() : base(DateTimeOffset.UtcNow) { }

    [JsonIgnore]
    public DateTimeOffset OccurredOn { get => Id; }

    public string Description { get; protected set; }

    public void EnsureDescription()
    {
        if (string.IsNullOrWhiteSpace(Description))
        {
            Description = DomainEventDescriptionBuilder.BuildDescription(this);
        }
    }

    public static DomainEvent? FromString(string message)
    {
        // // Uncomment to debug
        // Console.WriteLine(message);
        // return JsonSerializer.Deserialize<DomainEvent>(message, _jsonSerializerOptions);

        byte[] compressedData = Convert.FromBase64String(message);
        using var inputStream = new MemoryStream(compressedData);
        using var gzip = new GZipStream(inputStream, CompressionMode.Decompress);
        return JsonSerializer.Deserialize<DomainEvent>(gzip, BaseSourceGenerationContext.Default.DomainEvent);
    }

    public override string ToString()
    {
        // // Uncomment to debug
        // return JsonSerializer.Serialize(domainEvent, _jsonSerializerOptions);

        using var outputStream = new MemoryStream();
        using (var gzip = new GZipStream(outputStream, CompressionLevel.Optimal, leaveOpen: true))
        {
            using var writer = new Utf8JsonWriter(gzip);
            JsonSerializer.Serialize(writer, this, BaseSourceGenerationContext.Default.DomainEvent);
        }
        return Convert.ToBase64String(outputStream.ToArray());
    }

    public static implicit operator string(DomainEvent domainEvent) => domainEvent.ToString();

    public static implicit operator DomainEvent(string domainEvent) => FromString(domainEvent) 
        ?? throw new NullReferenceException($"Failed to convert string to {nameof(DomainEvent)}");
}