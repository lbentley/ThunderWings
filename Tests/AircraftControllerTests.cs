using Microsoft.AspNetCore.Mvc;
using Moq;
using ThunderWings.Controllers;
using ThunderWings.Interfaces;
using ThunderWings.Model;

namespace ThunderWings.Tests
{
    public class AircraftControllerTests
    {
        private readonly Mock<IAircraftService> _mockAircraftService;
        private readonly AircraftController _controller;

        public AircraftControllerTests()
        {
            _mockAircraftService = new Mock<IAircraftService>();
            _controller = new AircraftController(_mockAircraftService.Object);
        }

        [Fact]
        public async Task GetMilitaryJets_ReturnsOkResult_WithAircraftList()
        {
            // Arrange 
            var aircraftList = new List<Aircraft>
            {
                new Aircraft { Name = "F-22 Raptor", Manufacturer = "Lockheed Martin", Country = "USA", Role = "Fighter", TopSpeed = 1498, Price = 150000000 }
            };
            _mockAircraftService.Setup(service => service.GetAircraftAsync()).ReturnsAsync(aircraftList);

            // Act 
            var result = await _controller.GetMilitaryJets();

            // Assert 
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsType<List<Aircraft>>(okResult.Value);
            Assert.Single(returnedList);
        }
    }
}
