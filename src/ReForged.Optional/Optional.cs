namespace ReForged;

/// <summary>
/// Represents an optional value of a given type. An instance of this class either contains a value of type T or an exception.
/// </summary>
/// <typeparam name="T">The type of the value to be optionally contained.</typeparam>
public class Optional<T>
{
    private readonly T? _value;
    private readonly Exception? _exception;

    /// <summary>
    /// Initializes a new instance of the <see cref="Optional{T}"/> class.
    /// </summary>
    /// <param name="value">The value to be optionally contained.</param>
    /// <param name="exception">The exception to be optionally contained.</param>
    private Optional(T? value, Exception? exception)
    {
        _value = value;
        _exception = exception;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Optional{T}"/> class with a value.
    /// </summary>
    /// <param name="value">The value to be contained.</param>
    /// <returns>An instance of <see cref="Optional{T}"/> containing the provided value.</returns>
    public static Optional<T> Of(T value) => new Optional<T>(value, null);

    /// <summary>
    /// Creates a new instance of the <see cref="Optional{T}"/> class with an exception.
    /// </summary>
    /// <param name="exception">The exception to be contained.</param>
    /// <returns>An instance of <see cref="Optional{T}"/> containing the provided exception.</returns>
    public static Optional<T> OfException(Exception exception) => new Optional<T>(default, exception);

    /// <summary>
    /// Transforms the value in the <see cref="Optional{T}"/> instance using the provided mapping function.
    /// </summary>
    /// <typeparam name="TOut">The type of the result of the mapping function.</typeparam>
    /// <param name="mapper">A function that transforms the contained value.</param>
    /// <returns>An instance of <see cref="Optional{TOut}"/> containing the result of the mapping function.</returns>
    public Optional<TOut> Map<TOut>(Func<T, TOut> mapper) =>
        _exception is not null ? Optional<TOut>.OfException(_exception) : TryCreate(() => Optional<TOut>.Of(mapper(_value!)));

    /// <summary>
    /// Transforms the value in the <see cref="Optional{T}"/> instance using the provided binding function.
    /// </summary>
    /// <typeparam name="TOut">The type of the result of the binding function.</typeparam>
    /// <param name="binder">A function that transforms the contained value into an <see cref="Optional{TOut}"/> instance.</param>
    /// <returns>An instance of <see cref="Optional{TOut}"/> containing the result of the binding function.</returns>
    public Optional<TOut> Bind<TOut>(Func<T, Optional<TOut>> binder) =>
        _exception is not null ? Optional<TOut>.OfException(_exception) : TryCreate(() => binder(_value!));

    /// <summary>
    /// Returns the contained value if present, otherwise returns the provided default value.
    /// </summary>
    /// <param name="defaultValue">The value to be returned if the <see cref="Optional{T}"/> instance contains an exception.</param>
    /// <returns>The contained value if present, otherwise the provided default value.</returns>
    public T OrElse(T defaultValue) => _exception is not null ? defaultValue : _value!;

    /// <summary>
    /// Returns the contained value if present, otherwise throws the contained exception.
    /// </summary>
    /// <returns>The contained value if present.</returns>
    /// <exception cref="Exception">The contained exception if present.</exception>
    public T OrElseThrow() => _exception is not null ? throw _exception : _value!;
    
    /// <summary>
    /// Tries to create a new instance of the <see cref="Optional{TOut}"/> class using the provided function.
    /// </summary>
    /// <typeparam name="TOut">The type of the value to be optionally contained.</typeparam>
    /// <param name="func">A function that returns an <see cref="Optional{TOut}"/> instance.</param>
    /// <returns>An instance of <see cref="Optional{TOut}"/> containing the result of the function if successful, otherwise containing the thrown exception.</returns>
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