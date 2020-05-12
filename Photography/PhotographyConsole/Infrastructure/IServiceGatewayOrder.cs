using SharedModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotographyConsole.Infrastructure
{
    public interface IServiceGatewayOrder : IServiceGateway<OrderDTO>
    {
        public Task CreateOrder(ProductDTO productDTO, int customerID);
    }
}
