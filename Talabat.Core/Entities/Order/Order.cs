using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class Order:BaseEntity
    {
        public Order()
        {
        }

        public Order(string buyerEmail, Addresso shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItems> items, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }= DateTimeOffset.Now;
        public OrderStatus status { get; set; } = OrderStatus.Pending;
        public Addresso ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItems> Items { get; set; }=new HashSet<OrderItems>();
        public decimal SubTotal { get; set; }
        public string PaymentIntendId { get; set; }= string.Empty;
        
         
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;

    }
}
