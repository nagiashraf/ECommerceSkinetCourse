using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> GetBasketById (string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            return basket ?? new CustomerBasket(basketId);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket (CustomerBasket basket)
        {
            var updatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(basket);

            if (updatedBasket is null)
                return BadRequest("Error creating or updating basket");

            return updatedBasket;
        }

        [HttpDelete("{basketId}")]
        public async Task<IActionResult> DeleteBasket (string basketId)
        {
            var result = await _basketRepository.DeleteBasketAsync(basketId);

            if (!result)
                return BadRequest("Error deleting basket");

            return NoContent();
        }
    }
}