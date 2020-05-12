using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Data
{
    public interface IRepository<T>
    {
        public Task<T> Create(T obj);
        public Task<T> GetById(int id);
        public Task<List<T>> GetAll();
        public Task<T> Update(T obj);
        public Task<T> Delete(int id);
    }
}
