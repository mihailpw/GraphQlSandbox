using System;

namespace GQL.Annotations
{
    public interface IId
    {
        object ObjectValue { get; }
    }

    public struct Id<T> : IId, IEquatable<Id<T>>
    {
        public T Value { get; }

        object IId.ObjectValue => Value;


        public Id(T value)
        {
            Value = value;
        }


        public static implicit operator T(Id<T> id)
        {
            return id.Value;
        }

        public static implicit operator Id<T>(T value)
        {
            return new Id<T>(value);
        }

        public static bool operator ==(Id<T> id1, Id<T> id2)
        {
            return id1.Equals(id2);
        }

        public static bool operator !=(Id<T> id1, Id<T> id2)
        {
            return !id1.Equals(id2);
        }


        public override bool Equals(object other)
        {
            return other is Id<T> otherId && Equals(otherId);
        }

        public bool Equals(Id<T> other)
        {
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"Id({Value.ToString()})";
        }
    }
}