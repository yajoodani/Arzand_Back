using System;

namespace Arzand.Shared.Domain;

public abstract class Entity<T>
{
    public T Id { get; protected set; }
    public DateTime CreationDate { get; set;}
    public DateTime LastModifiedDate { get; set; }

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
