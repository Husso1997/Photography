using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductApi.Data
{
    public class ProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Dependency injected
        /// </summary>
        private readonly ProductApiContext _ctx;

        private readonly ILogger<Product> _logger;

        public ProductRepository(ProductApiContext context, ILogger<Product> logger)
        {
            _ctx = context;
            _logger = logger;
        }

        public async Task<Product> Create(Product obj)
        {
            try
            {
                var entityEntry = await _ctx.Products.AddAsync(obj).ConfigureAwait(false);
                await _ctx.SaveChangesAsync().ConfigureAwait(false);
                return entityEntry.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed creating product", ex);
                throw;
            }
        }

        public async Task<Product> Delete(int id)
        {
            try
            {
                var removedProduct = _ctx.Remove(new Product() { ID = id }).Entity;
                await _ctx.SaveChangesAsync().ConfigureAwait(false);
                return removedProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed deleting product", ex);
                throw;
            }
        }

        public async Task<List<Product>> GetAll()
        {
            try
            {
                return await _ctx.Products.ToListAsync().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                _logger.LogError("Failed retrieving products", ex);
                throw;
            }
        }

        public async Task<List<Product>> GetAllWhere(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                return await _ctx.Products.Where(predicate).ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed retrieving products", ex);
                throw;
            }
        }

        public async Task<Product> GetById(int id)
        {
            try
            {
                return await _ctx.Products.FirstOrDefaultAsync(x => x.ID == id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed retrieving product", ex);
                throw;
            }
        }

        public async  Task<Product> Update(Product product)
        {
            try
            {
                var updatedProduct = _ctx.Update(product).Entity;
                await _ctx.SaveChangesAsync().ConfigureAwait(false);
                return updatedProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed updating product", ex);
                throw;
            }
        }

        public async Task<List<Product>> UpdateRange(List<Product> products)
        {
            try
            {
                _ctx.UpdateRange(products);
                await _ctx.SaveChangesAsync().ConfigureAwait(false);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed updating product", ex);
                throw;
            }
        }
    }
}
