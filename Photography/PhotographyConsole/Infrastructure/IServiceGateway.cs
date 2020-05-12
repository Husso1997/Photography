using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotographyConsole.Infrastructure
{
    public interface IServiceGateway<T>
    {
        public Task<List<T>> GetAll();
    }
}
