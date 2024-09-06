using System;
using System.Collections.Generic;


namespace MengGrocery.Models
{
    public class CartItem
    {
        public int CartItemID { get; set; }
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal? PriceAtAdd { get; set; }
        public DateTime AddedAt { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
    }
}