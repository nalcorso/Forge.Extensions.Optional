#if false

using System.Diagnostics.CodeAnalysis;

namespace Forge.Common;

public static class OptionalExtensions
{

}

public static class Optional
{
    public static Optional<T> Some<T>(T value)
    {
        return new Optional<T>(value);
    }

    public static Optional<T> None<T>()
    {
        return new Optional<T>();
    }
    
    public static Optional<T> Error<T>(Exception exception)
    {
        return new Optional<T>(exception);
    }
 
}

public readonly partial struct Optional<T>
{
    private readonly T? _value;
    private readonly Exception? _exception;

    [MemberNotNullWhen(true, nameof(_value))]
    public bool HasValue { get; }
    public bool HasException => _exception is not null;
    
    internal Optional(T? value)
    {
        HasValue = value is not null;
        _value = value;
        _exception = null;
    }
    
    internal Optional(Exception exception)
    {
        HasValue = false;
        _value = default;
        _exception = exception;
    }
    
    public T? Release()
    {
        if (!HasValue)
            throw new InvalidOperationException("Optional does not have a value.");
        
        return _value;
    }

    public override string ToString() => 
        HasValue ? $"Optional[{_value}]" : "Optional.Empty";
    
    public T ValueOr(T other) =>
        HasValue ? _value : other;
    
    public T ValueOr(Func<T> other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));
        return HasValue ? _value : other();
    }
    
    public T ValueOrThrow()
    {
        if (HasValue)
            return _value;
        
        if (HasException)
            throw _exception!;
        
        throw new InvalidOperationException("Optional does not have a value.");
    }
    
    public Optional<T> Or(T other) =>
        HasValue ? this : new Optional<T>(other);
    
    public Optional<T> Or(Func<T> other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));
        return HasValue ? this : new Optional<T>(other());
    }
    
    public Optional<T> Or(Optional<T> other) =>
        HasValue ? this : other;
    
    public Optional<T> Or(Func<Optional<T>> other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));
        return HasValue ? this : other();
    }
    
    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
    {
        ArgumentNullException.ThrowIfNull(some, nameof(some));
        ArgumentNullException.ThrowIfNull(none, nameof(none));
        return HasValue ? some(_value) : none();
    }
    
    public void Match(Action<T> some, Action none)
    {
        ArgumentNullException.ThrowIfNull(some, nameof(some));
        ArgumentNullException.ThrowIfNull(none, nameof(none));
        if (HasValue)
            some(_value);
        else
            none();
    }
    
    public Optional<TResult> Map<TResult>(Func<T, TResult> map)
    {
        ArgumentNullException.ThrowIfNull(map, nameof(map));
        return HasValue ? new Optional<TResult>(map(_value)) : new Optional<TResult>();
    }
    
    public Optional<TResult> FlatMap<TResult>(Func<T, Optional<TResult>> map)
    {
        ArgumentNullException.ThrowIfNull(map, nameof(map));
        return HasValue ? map(_value) : new Optional<TResult>();
    }
    
    public Optional<T> Filter(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
        return HasValue && predicate(_value) ? this : new Optional<T>();
    }
    
    public Optional<TResult> Select<TResult>(Func<T, TResult> map)
    {
        ArgumentNullException.ThrowIfNull(map, nameof(map));
        return HasValue ? new Optional<TResult>(map(_value)) : new Optional<TResult>();
    }
}


public readonly partial struct Optional<T> : IEquatable<Optional<T>>
{
    public bool Equals(Optional<T> other)
    {
        if (!HasValue && !other.HasValue)
            return true;
        
        if (!HasValue || !other.HasValue)
            return false;
        
        return EqualityComparer<T>.Default.Equals(_value, other._value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Optional<T> other && Equals(other);
    }
    
    public override int GetHashCode()
    {
        return !HasValue ? 0
            : HashCode.Combine(_value);
    }

    public static bool operator ==(Optional<T> left, Optional<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Optional<T> left, Optional<T> right)
    {
        return !left.Equals(right);
    }
}

public readonly partial struct Optional<T> : IComparable<Optional<T>>
{
    public int CompareTo(Optional<T> other)
    {
        if (HasValue && !other.HasValue)
            return 1;
        
        if (!HasValue && other.HasValue)
            return -1;
        
        return Comparer<T>.Default.Compare(_value, other._value);
    }
    
    public static bool operator <(Optional<T> left, Optional<T> right) => 
        left.CompareTo(right) < 0;
    
    public static bool operator <=(Optional<T> left, Optional<T> right) =>
        left.CompareTo(right) <= 0;
    
    public static bool operator >(Optional<T> left, Optional<T> right) =>
        left.CompareTo(right) > 0;
    
    public static bool operator >=(Optional<T> left, Optional<T> right) =>
        left.CompareTo(right) >= 0;
}

#endif