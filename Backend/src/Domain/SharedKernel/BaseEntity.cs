using System.ComponentModel.DataAnnotations;
using Domain.SharedKernel.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Domain.SharedKernel;

// Abstract BaseEntity type which supports domain events
public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public delegate void DomainEventHandler(BaseEntity? sender, IDomainEvent e);
    protected static event DomainEventHandler? DomainEventRaised;

    protected BaseEntity() { }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
        DomainEventRaised?.Invoke(this, domainEvent);
    }

    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

// BaseEntity Generic to support getting the various ID
[PrimaryKey(nameof(Id))]
public abstract class BaseEntity<TId> : BaseEntity, IEquatable<BaseEntity<TId>> where TId : IEquatable<TId>
{
    [Key]
    [Required]
    public TId Id { get; protected set; }

    public new delegate void DomainEventHandler(BaseEntity<TId>? sender, IDomainEvent e);

    protected BaseEntity() { }

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