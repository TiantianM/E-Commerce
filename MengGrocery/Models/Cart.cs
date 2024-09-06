using System;
using System.Collections.Generic;

namespace MengGrocery.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public Guid CartGuidId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}