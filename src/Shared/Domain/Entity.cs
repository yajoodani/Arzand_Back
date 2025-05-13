using System;
using MediatR;

namespace Arzand.Shared.Domain;

public abstract class Entity<T>
{
    public T Id { get; protected set; }
    public DateTime CreationDate { get; set;}
    public DateTime LastModifiedDate { get; set; }

    private List<INotification>? _domainEvents;
    public IReadOnlyCollection<INotification>? DomainEvents => _domainEvents?.AsReadOnly();

    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents ??= new List<INotification>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<T> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (Id.Equals(default(T)) || other.Id.Equals(default(T))) return false;
        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity<T> left, Entity<T> right) => Equals(left, right);
    public static bool operator !=(Entity<T> left, Entity<T> right) => !Equals(left, right);
}
