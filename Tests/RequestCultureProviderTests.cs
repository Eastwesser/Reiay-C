using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class RequestCultureProviderTests
{
    private readonly Mock<ILogger<RequestCultureProvider>> _loggerMock;
    private readonly RequestCultureProvider _provider;

    public RequestCultureProviderTests()
    {
        _loggerMock = new Mock<ILogger<RequestCultureProvider>>();
        _provider = new RequestCultureProvider(_loggerMock.Object);
    }

    [Fact]
    public void DetermineRequestCulture_ValidCulture_ReturnsCorrectCulture()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Accept-Language"] = "en-US";

        // Act
        var result = _provider.DetermineRequestCulture(context);

        // Assert
        Assert.Equal("en-US", result.Name);
        _loggerMock.Verify(logger => logger.LogInformation("Язык, указанный в запросе: {Culture}", "en-US"), Times.Once);
    }

    [Fact]
    public void DetermineRequestCulture_InvalidCulture_ReturnsDefaultCulture()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Accept-Language"] = "invalid-culture";

        // Act
        var result = _provider.DetermineRequestCulture(context);

        // Assert
        Assert.Equal("ru", result.Name);
        _loggerMock.Verify(logger => logger.LogWarning("Неверный код языка в запросе: {Culture}. Установлен 'ru'", "invalid-culture"), Times.Once);
    }

    [Fact]
    public void DetermineRequestCulture_NoHeader_ReturnsDefaultCulture()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act
        var result = _provider.DetermineRequestCulture(context);

        // Assert
        Assert.Equal("ru", result.Name);
    }
}
