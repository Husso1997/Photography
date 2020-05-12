using OrderApi.Data;
using OrderApi.Models;
using System;
using System.Collections.Generic;

namespace OrderApi.Data
{
    public class DbSeeding : IDbSeeding
    {

        public void SeedWithData(OrderApiContext orderApiContext)
        {
            /// Deleting & Creating DB - memory
            orderApiContext.Database.EnsureDeleted();
            orderApiContext.Database.EnsureCreated();
            List<Order> orders = new List<Order>()
            {

            new Order()
                {
                    CustomerID = 1,
                    OrderProgress = OrderProgress.Accepted,
                    Products = new List<OrderProduct>(){new OrderProduct { ProductID = 1} }

                },

            new Order()
                {
                    CustomerID = 2,
                    OrderProgress = OrderProgress.Accepted,
                    Products = new List<OrderProduct>(){new OrderProduct { ProductID = 2} }
                }
        };
            orderApiContext.Orders.AddRange(orders);

            orderApiContext.SaveChanges();
        }
    }
}
