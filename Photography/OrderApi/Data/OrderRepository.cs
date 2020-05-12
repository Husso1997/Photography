using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Data
{
    public class OrderRepository : IRepository<Order>
    {
        /// <summary>
        /// Dependency injected
        /// </summary>
        private readonly OrderApiContext _ctx;

        private readonly ILogger<Order> _logger;

        public OrderRepository(OrderApiContext context, ILogger<Order> logger)
        {
            _ctx = context;
            _logger = logger;
        }

        public async Task<Order> Create(Order obj)
        {
            try
            {
                var entityEntry = await _ctx.Orders.AddAsync(obj).ConfigureAwait(false);
                await _ctx.SaveChangesAsync().ConfigureAwait(false);
                return entityEntry.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed creating Order", ex);
                throw;
            }
        }

        public async Task<Order> Delete(int id)
        {
            try
            {
                var removedOrder = _ctx.Remove(new Order() { ID = id }).Entity;
                await _ctx.SaveChangesAsync().ConfigureAwait(false);
                return removedOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed deleting Order", ex);
                throw;
            }
        }

        public async Task<List<Order>> GetAll()
        {
            try
            {
                return await _ctx.Orders.Include(x => x.Products).ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed retrieving Orders", ex);
                throw;
            }
        }

        public async Task<Order> GetById(int id)
        {
            try
            {
                return await _ctx.Orders.Include(x => x.Products).FirstOrDefaultAsync(x => x.ID == id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed retrieving Order", ex);
                throw;
            }
        }

        public async Task<Order> Update(Order order)
        {
            try
            {
                var updatedOrder = _ctx.Update(order).Entity;
                await _ctx.SaveChangesAsync().ConfigureAwait(false);
                return updatedOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed updating Order", ex);
                throw;
            }
        }
    }
}
