using Microsoft.AspNetCore.Mvc;
using ThunderWings.Interfaces;
using ThunderWings.Model;

namespace ThunderWings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {        
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {            
            _basketService = basketService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToBasket([FromBody] string aircraftName)
        {
            if (string.IsNullOrEmpty(aircraftName))
            {
                return BadRequest("Aircraft name cannot be null or empty.");
            }

            try
            {
                var response = await _basketService.AddAircraftToBasketAsync(aircraftName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromBasket([FromBody] string aircraftName)
        {
            if (string.IsNullOrEmpty(aircraftName))
            {
                return BadRequest("Aircraft name cannot be null or empty.");
            }

            try
            {
                var response = await _basketService.RemoveAircraftFromBasketAsync(aircraftName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var response = await _basketService.Checkout();                 
                await _basketService.SaveBasketAsync(new Basket()); // Clear basket
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
