namespace ReForged;

public class Optional<T>
{
    private readonly T? _value;
    private readonly Exception? _exception;

    private Optional(T? value, Exception? exception)
    {
        _value = value;
        _exception = exception;
    }

    public static Optional<T> Of(T value) => new Optional<T>(value, null);

    public static Optional<T> OfException(Exception exception) => new Optional<T>(default, exception);

    public Optional<TOut> Map<TOut>(Func<T, TOut> mapper) =>
        _exception is not null ? Optional<TOut>.OfException(_exception) : TryCreate(() => Optional<TOut>.Of(mapper(_value!)));

    public Optional<TOut> Bind<TOut>(Func<T, Optional<TOut>> binder) =>
        _exception is not null ? Optional<TOut>.OfException(_exception) : TryCreate(() => binder(_value!));

    public T OrElse(T defaultValue) => _exception is not null ? defaultValue : _value!;

    public T OrElseThrow() => _exception is not null ? throw _exception : _value!;
    
    public static Optional<TOut> TryCreate<TOut>(Func<Optional<TOut>> func)
    {
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            return Optional<TOut>.OfException(ex);
        }
    }
}