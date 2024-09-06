using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MengGrocery.Models
{
    public class Order
    {
        public int? OrderID { get; set; }
       
        public int? CartID { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? ShippingPrice { get; set; }
        public decimal? Coupon { get; set; }
        public decimal? NetAmount { get; set; }
        public string? OrderStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ShippedAt { get; set; }
    }
}