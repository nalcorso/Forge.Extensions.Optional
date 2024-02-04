namespace ReForged.Optional.Tests;

using ReForged;

public class OptionalTests
{
    [Fact]
    public void Of_ShouldReturnOptionalWithValue()
    {
        // Arrange
        int expectedValue = 5;

        // Act
        var optional = Optional<int>.Of(expectedValue);

        // Assert
        Assert.Equal(expectedValue, optional.OrElse(0));
    }

    [Fact]
    public void OfException_ShouldReturnOptionalWithException()
    {
        // Arrange
        var expectedException = new Exception("Test exception");

        // Act
        var optional = Optional<int>.OfException(expectedException);

        // Assert
        var actualException = Assert.Throws<Exception>(() => optional.OrElseThrow());
        Assert.Equal(expectedException.Message, actualException?.Message);
    }

    [Fact]
    public void Map_ShouldTransformValue()
    {
        // Arrange
        var optional = Optional<int>.Of(5);
        Func<int, string> mapper = i => i.ToString();

        // Act
        var result = optional.Map(mapper);

        // Assert
        Assert.Equal("5", result.OrElse(""));
    }

    [Fact]
    public void Bind_ShouldTransformValueToOptional()
    {
        // Arrange
        var optional = Optional<int>.Of(5);
        Func<int, Optional<string>> binder = i => Optional<string>.Of(i.ToString());

        // Act
        var result = optional.Bind(binder);

        // Assert
        Assert.Equal("5", result.OrElse(""));
    }

    [Fact]
    public void OrElse_ShouldReturnDefaultValueWhenExceptionIsPresent()
    {
        // Arrange
        var expectedException = new Exception("Test exception");
        var optional = Optional<int>.OfException(expectedException);

        // Act
        var result = optional.OrElse(10);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void OrElseThrow_ShouldThrowExceptionWhenExceptionIsPresent()
    {
        // Arrange
        var expectedException = new Exception("Test exception");
        var optional = Optional<int>.OfException(expectedException);

        // Act & Assert
        var actualException = Assert.Throws<Exception>(() => optional.OrElseThrow());
        Assert.Equal(expectedException.Message, actualException?.Message);
    }
    
    [Fact]
    public void Try_ShouldReturnOptionalWithExceptionWhenFunctionThrows()
    {
        // Arrange
        var expectedException = new Exception("Test exception");
        Func<Optional<int>> func = () => throw expectedException;

        // Act
        var result = Optional<int>.TryCreate(func);

        // Assert
        var actualException = Assert.Throws<Exception>(() => result.OrElseThrow());
        Assert.Equal(expectedException.Message, actualException?.Message);
    }
}