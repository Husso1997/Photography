using System;
using System.Collections.Generic;

namespace CustomerApi.Data
{
    public class DbSeeding : IDbSeeding
    {

        public void SeedWithData(CustomerApiContext CustomerApiContext)
        {
            /// Deleting & Creating DB - memory
            CustomerApiContext.Database.EnsureDeleted();
            CustomerApiContext.Database.EnsureCreated();
            List<Customer> Customers = new List<Customer>()
            {

            new Customer()
                {
                    Name = "Bent Jorgen",
                    VATNumber = "12345678"
                },

            new Customer()
                {

                    Name = "Hussain Larsen",
                    VATNumber = "87654321"
                }
        };
            CustomerApiContext.Customers.AddRange(Customers);

            CustomerApiContext.SaveChanges();
        }
    }
}
