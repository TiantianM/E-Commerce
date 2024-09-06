using System;
using System.Data;
using Dapper;
using MengGrocery.Models;
using MySql.Data.MySqlClient;

namespace MengGrocery.DAL
{
	public interface IProductQuery
	{
		List<ProductModel> GetProduct();
	}
	public class ProductQuery : IProductQuery
	{
		private readonly IConfiguration _configuration;

       

        public ProductQuery(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public List<ProductModel> GetProduct()
		{
			using (IDbConnection db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				string sqlQuery = "select Name as ProductName, ImageUrl, ProductId, Price, Category from MengGrocery.GroceryProduct";
                return db.Query<ProductModel>(sqlQuery).ToList();

            }

		}

	}
}



