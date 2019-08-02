using System;

namespace GQL.Services.Infra
{
    public struct NonNull<T> : IEquatable<NonNull<T>>
    {
        public T Value { get; }


        public NonNull(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }


        public static implicit operator T(NonNull<T> nonNull)
        {
            return nonNull.Value;
        }

        public static implicit operator NonNull<T>(T value)
        {
            return new NonNull<T>(value);
        }

        public static bool operator ==(NonNull<T> nonNull1, NonNull<T> nonNull2)
        {
            return nonNull1.Equals(nonNull2);
        }

        public static bool operator !=(NonNull<T> nonNull1, NonNull<T> nonNull2)
        {
            return !nonNull1.Equals(nonNull2);
        }


        public override bool Equals(object other)
        {
            return other is NonNull<T> otherNonNull && Equals(otherNonNull);
        }

        public bool Equals(NonNull<T> other)
        {
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"NonNull({Value.ToString()})";
        }
    }
}