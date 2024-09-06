using Dapper;
using System;
using System.Data;
using MengGrocery.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg.OpenPgp;


namespace MengGrocery.DAL
{
    public interface ICartQuery
    {
    
        void AddCartItem(string cartGuidId, int productId, int quantity);
        void UpdateCartItem(string cartGuidId, int productId, int quantity);
        void DeleteCartItem(string cartGuidId, int productId);
        Cart GetCartByGuid(string cartGuid);
        int GetCartItemCount(string cartGuidId);
        Cart GetCart(string cartId);
    }

    public class CartQuery : ICartQuery
    {
        private readonly IConfiguration _configuration;

        public CartQuery(IConfiguration configuration)
        {
            _configuration = configuration;
        }




        public void AddCartItem (string cartGuidId, int productId, int quantity)
        {
            using (IDbConnection db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var q1 = @"INSERT INTO MengGrocery.Cart (CartGuidId)
SELECT @CartGuidId
WHERE NOT EXISTS (SELECT 1 FROM MengGrocery.Cart WHERE CartGuidId = @CartGuidId);

SELECT CartID FROM MengGrocery.Cart WHERE CartGuidId = @CartGuidId;";

                var cartId = db.QueryFirstOrDefault<int>(q1, new { CartGuidId = cartGuidId });

                //first check if such product already exists in the cart
                var q2 = @"SELECT 1 FROM MengGrocery.CartItem WHERE CartID = @CartID AND ProductID = @ProductID";
                var exists = db.QueryFirstOrDefault<int>(q2, new { CartID = cartId, ProductID = productId });

                // if exists, increment the quantity by 1
                if (exists == 1)
                {
                    var q3 = @"UPDATE MengGrocery.CartItem SET Quantity = Quantity + @Quantity WHERE CartID = @CartID AND ProductID = @ProductID";
                    db.Execute(q3, new { CartID = cartId, ProductID = productId, Quantity = quantity });
                    return;
                }

                //if not exists, add new item to the cart, the column PriceAtAdd is from joining the GroceryProduct table



                var query = @"INSERT INTO MengGrocery.CartItem (CartID, ProductID, Quantity, PriceAtAdd)
VALUES (@CartID, @ProductID, @Quantity, (SELECT Price FROM MengGrocery.GroceryProduct WHERE ProductID = @ProductID))";


                db.Execute(query, new { CartID = cartId, ProductID = productId, Quantity = quantity });
                
            }
        }


        public void UpdateCartItem(string cartGuidId, int productId, int quantity)
        {
            using (IDbConnection db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                    var q1 = @"SELECT CartId FROM MengGrocery.Cart WHERE CartGuidId = @CartGuidId";
                var cartId = db.QueryFirstOrDefault<int>(q1, new { CartGuidId = cartGuidId });

                if (cartId == 0)
                {
                    return;
                }


                var query = @"UPDATE MengGrocery.CartItem SET Quantity = @Quantity WHERE CartID = @CartID AND ProductID = @ProductID";

                db.Execute(query, new { CartID = cartId, ProductID = productId, Quantity = quantity });
                
                
            }
        }

        public void DeleteCartItem(string cartGuidId, int productId)
        {
            using (IDbConnection db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var q1 = @"SELECT CartId FROM MengGrocery.Cart WHERE CartGuidId = @CartGuidId";
                var cartId = db.QueryFirstOrDefault<int>(q1, new { CartGuidId = cartGuidId });

                if (cartId == 0)
                {
                    return;
                }

                var query = @"DELETE FROM MengGrocery.CartItem WHERE CartID = @CartID AND ProductID = @ProductID";

                db.Execute(query, new { CartID = cartId, ProductID = productId });
            }
        }


        public Cart GetCartByGuid(string cartGuid)
        {
            using (IDbConnection db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var cart = db.Query<Cart>("SELECT * FROM Cart WHERE CartGuidId = @CartGuidId", new { CartGuidId = cartGuid }).FirstOrDefault();
                if (cart != null)
                {
                    string sqlQuery = @"
                        SELECT ci.*, gp.Name AS ProductName, gp.Price, gp.ImageUrl
                        FROM CartItem ci
                        INNER JOIN GroceryProduct gp ON ci.ProductID = gp.ProductID
                        WHERE ci.CartID = @CartID";
                    cart.CartItems = db.Query<CartItem>(sqlQuery, new { CartID = cart.CartId }).ToList();
                }
                return cart;
            }
        }

        public Cart? GetCart(string cartId)
{
    using (IDbConnection db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
    {
        var query = "SELECT * FROM Cart WHERE CartGuidId = @CartGuidId";
        var cart = db.QueryFirstOrDefault<Cart>(query, new { CartGuidId = cartId });
        if (cart != null)
        {
            cart.CartItems = db.Query<CartItem>(
                @"SELECT 
c.Quantity,
c.ProductId,
c.AddedAt,
p.Name AS ProductName,
p.Price
FROM CartItem c
JOIN Product p ON p.ProductId = c.ProductId
WHERE CartId = @CartId", new { CartId = cart.CartId }).ToList();
        }

        return cart;
    }
}




        public int GetCartItemCount(string cartGuidId)
        {
            using (IDbConnection db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var q1 = @"SELECT CartId FROM MengGrocery.Cart WHERE CartGuidId = @CartGuidId";
                var cartId = db.QueryFirstOrDefault<int>(q1, new { CartGuidId = cartGuidId });

                if (cartId == 0)
                {
                    return 0;
                }

                var q2 = @"SELECT SUM(Quantity) FROM MengGrocery.CartItem WHERE CartID = @CartID";
             var count = db.QueryFirstOrDefault<int?>(q2, new { CartID = cartId });

                //return count;
                return count ?? 0;
            }
        }




    }
                


       

       
    
}