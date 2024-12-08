using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order;

namespace Talabat.APIs.DTOs
{
    public class OrderDTO
    {
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethod { get; set; }
        [Required]
        public AddressDTO ShippingAddress { get; set; }
    }
}
