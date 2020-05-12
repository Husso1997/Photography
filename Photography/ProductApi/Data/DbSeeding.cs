using ProductApi.Models;
using System;
using System.Collections.Generic;

namespace ProductApi.Data
{
    public class DbSeeding : IDbSeeding
    {

        public void SeedWithData(ProductApiContext productApiContext)
        {
            /// Deleting & Creating DB - memory
            productApiContext.Database.EnsureDeleted();
            productApiContext.Database.EnsureCreated();
            List<Product> prods = new List<Product>() { new Product()
            {
                Description = "5 Photos & 20 minutes of filming.",
                Name = "Standard pack",
                Price = 500
            },

            new Product()
            {
                Description = "10 Photo & 50 minutes of filming.",
                Name = "Luxury Pack",
                Price = 1000
            }
        };
            productApiContext.Products.AddRange(prods);

            productApiContext.SaveChanges();
        }
    }
}
