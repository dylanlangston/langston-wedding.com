using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.SharedKernel.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Domain.SharedKernel;

// Abstract BaseEntity type which supports domain events
public abstract class BaseEntity
{
    private List<DomainEvent> _domainEvents = new();

    private void EnsureDomainEventsListExists()
    {
        // This happens when RuntimeHelpers.GetUninitializedObject is used to create a BaseEntity
        // This check ensures that events can still be dispacted for this type
        if (_domainEvents == null)
        {
            _domainEvents = new();
        }
    }

    [NotMapped]
    [JsonIgnore]
    public IReadOnlyList<DomainEvent> DomainEvents
    {
        get
        {
            EnsureDomainEventsListExists();
            return _domainEvents.AsReadOnly();
        }
    }

    public delegate void DomainEventHandler(BaseEntity? sender, DomainEvent e);
    protected static event DomainEventHandler? DomainEventRaised;

    protected BaseEntity() { }

    public void RaiseDomainEvent(DomainEvent domainEvent)
    {
        domainEvent.EnsureDescription();

        EnsureDomainEventsListExists();
        _domainEvents.Add(domainEvent);
        DomainEventRaised?.Invoke(this, domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        EnsureDomainEventsListExists();
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        EnsureDomainEventsListExists();
        _domainEvents.Clear();
    }
}

// BaseEntity Generic to support getting the various ID
[PrimaryKey(nameof(Id))]
public abstract class BaseEntity<TId> : BaseEntity, IEquatable<BaseEntity<TId>> where TId : IEquatable<TId>
{
    [Key]
    public TId Id { get; protected init; }

    public new delegate void DomainEventHandler(BaseEntity<TId>? sender, IDomainEvent e);

    private BaseEntity() { }

    protected BaseEntity(TId id)
    {
        if (EqualityComparer<TId>.Default.Equals(id, default))
        {
            throw new ArgumentException($"Entity ID cannot be the default value for type {typeof(TId).Name}.", nameof(id));
        }
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || !(obj is BaseEntity<TId> other))
        {
            return false;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        bool thisIsTransient = EqualityComparer<TId>.Default.Equals(Id, default);
        bool otherIsTransient = EqualityComparer<TId>.Default.Equals(other.Id, default);

        if (thisIsTransient && otherIsTransient)
        {
            return ReferenceEquals(this, other);
        }
        if (thisIsTransient || otherIsTransient)
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        if (EqualityComparer<TId>.Default.Equals(Id, default))
        {
            return base.GetHashCode();
        }
        return (GetType().GetHashCode() * 907) + Id.GetHashCode();
    }

    public bool Equals(BaseEntity<TId>? other) => Equals((object?)other);


    public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        if (ReferenceEquals(left, null))
        {
            return ReferenceEquals(right, null);
        }
        return left.Equals(right);
    }

    public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right) => !(left == right);
}