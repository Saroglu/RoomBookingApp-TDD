using Microsoft.Extensions.Logging;
using Moq;
using RoomBookingApp.Api.Controllers;
using Shouldly;
using Xunit;

namespace RoomBookingApp.Api.Tests
{

    public class TestTheWeatherForecast
    {
        [Fact]
        public void Should_Return_Value()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBeGreaterThan(1);
        }
    }
}
