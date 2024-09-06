using System;
namespace MengGrocery.Models
{
	public class ProductModel
	{
		public string ProductName { get; set; }
		
		public int ProductId { get; set; }
		public decimal Price { get; set; }
		public string ImageUrl { get; set; }
		public string Description { get; set; }
		public decimal StockQuantity { get; set; }
		public string Category { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public DateOnly ExpirationDate { get; set; }
		public string Unit { get; set; }
		public string Ingredients { get; set; }


		
	}
}

