# ReForge.Optional

![GitHub License](https://img.shields.io/github/license/nalcorso/ReForge.Optional)
![NuGet Version](https://img.shields.io/nuget/v/ReForge.Optional)

> ⚠️ This project is in the early stages of development and should be used with caution. It is currently slower, uses more memory and is less feature-rich than other libraries for implementing Discriminated Unions in C#. Please consider using [OneOf](https://github.com/mcintyre321/OneOf/) for production use.

ReForge.Optional is a minimal discriminated union for C# that provides a simple and intuitive way to represent optional values. It is intended for use in projects that require a lightweight and easy-to-use library for working with optional values.

## Alternatives

[OneOf](https://github.com/mcintyre321/OneOf/) is a mature and widely used library for implementing Discriminated Unions in C#. This is the preferred choice for most projects.

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