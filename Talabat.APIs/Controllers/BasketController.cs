using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.RepostriesInterfaces;

namespace Talabat.APIs.Controllers
{
    

    public class BasketController : BaseAPIController
    {
        private readonly IBasketRepositry _basketRepositry;

        public BasketController(IBasketRepositry basketRepositry) 
        {
           _basketRepositry = basketRepositry;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
           var basket =await _basketRepositry.GetBasketAsync(id);
            if (basket is null) return Ok( new CustomerBasket() { Id = id });
            return Ok(basket);
        }


        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket(CustomerBasket basket)
        {
            var CreateOrUpdateBasket = await _basketRepositry.UpdateBasketAsync(basket);
            if (CreateOrUpdateBasket is null) return BadRequest(error: new ApiResponse(statusCode: 400));
            return Ok(CreateOrUpdateBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
           await _basketRepositry.DeleteBasketAsync(id);
        }
    }
}
