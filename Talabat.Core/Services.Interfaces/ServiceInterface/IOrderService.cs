using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Services.Interfaces.ServiceInterface
{
    public interface IOrderService
    {

        Task<Order?> CreateOrderAsync(string byuerEmail, string basketId, int deliverymethod, Addresso shippingAdderss);
        Task<IReadOnlyList<Order>?> GetOrdersForSpecificUserAsync(string byuerEmail);
        Task<Order?> GetOrderByIdForSpecificUserAsync(string byuerEmail,int orderId);


    }
}
