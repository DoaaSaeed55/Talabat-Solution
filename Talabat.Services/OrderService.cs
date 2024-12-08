using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;
using Talabat.Core.RepostriesInterfaces;
using Talabat.Core.Services.Interfaces.ServiceInterface;
using Talabat.Repositry;
using Talabat.Repositry.Repositres;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepositry _basketRepositry;
        private readonly IGenericRepositry<DeliveryMethod> _deliveryRepo;
        private readonly IUnitOfWork _UnitOWork;
        
        public OrderService(IBasketRepositry basketRepositry,IGenericRepositry<DeliveryMethod> deliveryRepo,
            IUnitOfWork UnitOWork)
        {
            _basketRepositry = basketRepositry;
            _deliveryRepo = deliveryRepo;
            _UnitOWork = UnitOWork;
        }
        public async Task<Order?> CreateOrderAsync(string byuerEmail, string basketId, int deliverymethodID, Addresso shippingAdderss)
        {
            var basket = await _basketRepositry.GetBasketAsync(basketId);
            var orderItems=new List<OrderItems>();
            if (basket?.Items?.Count > 0)
            {
                foreach(var item in basket.Items)
                {
                    var product =await _UnitOWork.Repositry<Product>().GetAsync(item.Id);
                    var productItemOrder = new ProductItemOrder(product.Id,product.Name,product.PictureUrl);
                    var OrderItem = new OrderItems(productItemOrder, item.Price, item.Quantity);

                    orderItems.Add(OrderItem);
                }
            }
            var subTotal=orderItems.Sum(x => x.Price*x.Quantity);

            var deliMethod = await _deliveryRepo.GetAsync(deliverymethodID);
            //var deliMethod = await _UnitOWork.Repositry<DeliveryMethod>().GetAsync(deliverymethodID);

            var order=new Order(byuerEmail, shippingAdderss, deliMethod, orderItems,subTotal);

            await _UnitOWork.Repositry<Order>().AddAsync(order);

            var result =await _UnitOWork.CompleteAsync();
            if (result <= 0) return null;

            return order;
        }

        public Task<Order?> GetOrderByIdForSpecificUserAsync(string byuerEmail, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>?> GetOrdersForSpecificUserAsync(string byuerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
