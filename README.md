# ReForge.Optional

ReForge.Optional is a minimal and non-optimized implementation of the Optional type in C#. It provides a way to represent optional values without using null references.

## Installation

To use ReForge.Optional in your project, add it as a reference.

## Usage

Here are some examples of how to use the `Optional<T>` class:

### Creating an Optional

```csharp
int value = 5;
var optional = Optional<int>.Of(value);
```

### Creating an Optional with an Exception

```csharp
var exception = new Exception("Test exception");
var optional = Optional<int>.OfException(exception);
```

### Mapping an Optional

```csharp
var optional = Optional<int>.Of(5);
Func<int, string> mapper = i => i.ToString();
var result = optional.Map(mapper);
```

### Binding an Optional

```csharp
var optional = Optional<int>.Of(5);
Func<int, Optional<string>> binder = i => Optional<string>.Of(i.ToString());
var result = optional.Bind(binder);
```

### Using OrElse

```csharp
var exception = new Exception("Test exception");
var optional = Optional<int>.OfException(exception);
var result = optional.OrElse(10);
```

### Using OrElseThrow

```csharp
var exception = new Exception("Test exception");
var optional = Optional<int>.OfException(exception);
Exception? actualException = Assert.Throws<Exception>(() => optional.OrElseThrow());
```

### Using TryCreate

```csharp
var exception = new Exception("Test exception");
Func<Optional<int>> func = () => throw exception;
var result = Optional<int>.TryCreate(func);
```

## Contributing

Contributions are welcome. Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License.