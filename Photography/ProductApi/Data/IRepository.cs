using ProductApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductApi.Data
{
    public interface IRepository<T>
    {
        public Task<T> Create(T obj);
        public Task<T> GetById(int id);
        public Task<List<T>> GetAll();
        public Task<List<T>> GetAllWhere(Expression<Func<Product, bool>> predicate);

        public Task<T> Update(T obj);
        public Task<List<T>> UpdateRange(List<T> obj);
        public Task<T> Delete(int id);

    }
}
