using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerApi.Data
{
    public class CustomerRepository : IRepository<Customer>
    {
        /// <summary>
        /// Dependency injected
        /// </summary>
        private readonly CustomerApiContext _ctx;

        private readonly ILogger<Customer> _logger;

        public CustomerRepository(CustomerApiContext context, ILogger<Customer> logger)
        {
            _ctx = context;
            _logger = logger;
        }

        public async Task<Customer> Create(Customer obj)
        {
            try
            {
                var entityEntry = await _ctx.Customers.AddAsync(obj).ConfigureAwait(false);
                return entityEntry.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed creating Customer", ex);
                throw;
            }
        }

        public async Task<Customer> Delete(int id)
        {
            try
            {
                var removedCustomer = _ctx.Remove(new Customer() { ID = id }).Entity;
                await _ctx.SaveChangesAsync().ConfigureAwait(false);
                return removedCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed deleting Customer", ex);
                throw;
            }
        }

        public async Task<List<Customer>> GetAll()
        {
            try
            {
                return await _ctx.Customers.ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed retrieving Customers", ex);
                throw;
            }
        }

        public async Task<Customer> GetById(int id)
        {
            try
            {
                return await _ctx.Customers.FirstOrDefaultAsync(x => x.ID == id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed retrieving Customer", ex);
                throw;
            }
        }

        public async Task<Customer> Update(Customer Customer)
        {
            try
            {
                var updatedCustomer = _ctx.Update(Customer).Entity;
                await _ctx.SaveChangesAsync().ConfigureAwait(false);
                return updatedCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed updating Customer", ex);
                throw;
            }
        }
    }
}
