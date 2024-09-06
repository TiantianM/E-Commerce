using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MengGrocery.Models;
using System.Data;
using MySql.Data.MySqlClient;

namespace MengGrocery.DAL
{
        public interface IAccountQuery
    {
        List<Order> GetRecentOrders(string email);
    }
    
    public class AccountQuery : IAccountQuery
    {
        private readonly IConfiguration _configuration;

        public AccountQuery(IConfiguration configuration)
		{
			_configuration = configuration;
		}

        public List<Order> GetRecentOrders(string email)
        {
            using (IDbConnection db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"SELECT 
                                o.OrderID,
                                o.CartID,
                                o.TotalAmount,
                                o.Tax,
                                o.ShippingPrice,
                                o.Coupon,
                                o.OrderStatus,
                                o.CreatedAt,
                                o.ShippedAt
                            FROM 
                                MengGrocery.Order o
                            JOIN 
                                MengGrocery.OrderCustomer c 
                            ON 
                                c.OrderId = o.OrderID
                            WHERE 
                                c.email = @email 
                                AND o.CreatedAt > NOW() - INTERVAL 1 YEAR 
                            ORDER BY 
                                o.CreatedAt DESC
                            LIMIT 1000;";
                return db.Query<Order>(sqlQuery, new { Email = email }).ToList();
            }
        }
    }

}
