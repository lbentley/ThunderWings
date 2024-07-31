using Microsoft.AspNetCore.Mvc;
using Moq;
using ThunderWings.Controllers;
using ThunderWings.Interfaces;

namespace ThunderWings.Tests
{
    public class BasketControllerTests
    {
        private readonly Mock<IBasketService> _mockBasketService;
        private readonly BasketController _controller;

        public BasketControllerTests()
        {
            _mockBasketService = new Mock<IBasketService>();
            _controller = new BasketController(_mockBasketService.Object);
        }

        [Fact]
        public async Task AddToBasket_ValidAircraftName_ReturnsOkResult()
        {
            // Arrange 
            string aircraftName = "F-22 Raptor";
            _mockBasketService.Setup(service => service.AddAircraftToBasketAsync(aircraftName)).ReturnsAsync("Successfully added aircraft to basket");

            // Act 
            var result = await _controller.AddToBasket(aircraftName);

            // Assert 
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully added aircraft to basket", okResult.Value);
        }

        [Fact]
        public async Task AddToBasket_EmptyAircraftName_ReturnsBadRequest()
        {
            // Act 
            var result = await _controller.AddToBasket("");

            // Assert 
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Aircraft name cannot be null or empty.", badRequestResult.Value);
        }

    }
}
