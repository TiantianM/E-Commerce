using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MengGrocery.Models;
using System.Data;


namespace MengGrocery.DAL
{
    public interface IOrderQuery
    {
        OrderResult PlaceOrder(Order order, List<OrderItem> orderItems, OrderCustomer orderCustomer);
    }

    public class OrderQuery : IOrderQuery
    {
        private readonly IConfiguration _configuration;

        public OrderQuery(IConfiguration configuration)
		{
			_configuration = configuration;
		}

        public OrderResult PlaceOrder(Order order, List<OrderItem> orderItems, OrderCustomer orderCustomer)
        {
            using (IDbConnection db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (var transaction = db.BeginTransaction())
                {

                    try
                    {
                        var q1 = @"INSERT INTO MengGrocery.Order (CartID, TotalAmount, Tax, ShippingPrice, Coupon, OrderStatus, CreatedAt)
                                VALUES (@CartID, @TotalAmount, @Tax, @ShippingPrice, @Coupon, @OrderStatus, @CreatedAt)";
                        db.Execute(q1, 
                            new { CartID = order.CartID, TotalAmount = order.TotalAmount, Tax = order.Tax, ShippingPrice = order.ShippingPrice, 
                                Coupon = order.Coupon, OrderStatus = order.OrderStatus, CreatedAt = order.CreatedAt },
                            transaction);

                        var orderId = db.QuerySingleOrDefault<int>("SELECT LAST_INSERT_ID()", transaction: transaction);

                        foreach (var item in orderItems)
                        {
                            item.OrderID = orderId;
                            var q2 = @"INSERT INTO MengGrocery.OrderItem (OrderID, ProductID, Quantity, UnitPrice)
                                    VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice)";
                            db.Execute(q2, new { OrderID = orderId, item.ProductID, item.Quantity, item.UnitPrice }, transaction);
                        }

                        var q3 = @"INSERT INTO MengGrocery.OrderCustomer (OrderId, Email, ShippingFirstName, ShippingLastName, ShippingAddress, ShippingCity, ShippingState, ShippingPostalCode, ShippingPhoneNumber, ShippingCountry, BillingFirstName, BillingLastName, BillingAddress, BillingCity, BillingState, BillingZipCode, BillingPhoneNumber, BillingCountry)
                                VALUES (@OrderId, @Email, @ShippingFirstName, @ShippingLastName, @ShippingAddress, @ShippingCity, @ShippingState, @ShippingZipCode, @ShippingPhoneNumber, @ShippingCountry, @BillingFirstName, @BillingLastName, @BillingAddress, @BillingCity, @BillingState, @BillingZipCode, @BillingPhoneNumber, @BillingCountry)";
                        db.Execute(q3, new { orderId, orderCustomer.Email, orderCustomer.ShippingFirstName, orderCustomer.ShippingLastName, orderCustomer.ShippingAddress, orderCustomer.ShippingCity, orderCustomer.ShippingState, orderCustomer.ShippingZipCode, orderCustomer.ShippingPhoneNumber, orderCustomer.ShippingCountry, 
                                                    orderCustomer.BillingFirstName, orderCustomer.BillingLastName, orderCustomer.BillingAddress, orderCustomer.BillingCity, 
                                                    orderCustomer.BillingState, orderCustomer.BillingZipCode, orderCustomer.BillingPhoneNumber, orderCustomer.BillingCountry }, transaction);

                        transaction.Commit();
                        return new OrderResult { Success = true, Message = "Order placed successfully" };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new OrderResult { Success = false, Message = ex.Message };
                    }

                }
                
                

            }
        }
    }
}


