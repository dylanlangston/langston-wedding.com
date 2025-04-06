using System.Reflection;

namespace Domain.SharedKernel.DomainEvents;

public static class DomainEventDescriptionBuilder
{
    public static string BuildDescription(IDomainEvent domainEvent)
    {
        if (domainEvent == null)
            throw new ArgumentNullException(nameof(domainEvent));

        var description = $"{domainEvent.GetType().Name} occurred on {domainEvent.OccurredOn:o}";

        var additionalProperties = domainEvent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in additionalProperties)
        {
            if (prop.Name == nameof(DomainEvent.OccurredOn) || 
                prop.Name == nameof(DomainEvent.Description) || 
                prop.Name == nameof(DomainEvent.DomainEvents) ||
                prop.Name == nameof(DomainEvent.Id))
                continue;

            var value = prop.GetValue(domainEvent, null);
            if (value != null)
                description += $", {prop.Name}: {value}";
        }
        
        return description;
    }
}
