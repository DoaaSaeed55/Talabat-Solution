using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order;
using Talabat.Core.Services.Interfaces.ServiceInterface;

namespace Talabat.APIs.Controllers
{
    
    public class OrderController : BaseAPIController
    {
        private readonly IOrderService _order;
        private readonly IMapper _mapper;

        public OrderController(IOrderService order,IMapper mapper) 
        {
            _order = order;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO model)
        {
            var buyerEmail= "Do@gmail.com";
            var address= _mapper.Map<AddressDTO, Addresso>(model.ShippingAddress);
            var order=_order.CreateOrderAsync(buyerEmail, model.BasketId,model.DeliveryMethod,address);
            if (order is null) return BadRequest(new ApiResponse(statusCode:400,message:"There is a problem with your Order!"));
            return Ok(order);
        }
    }
}
