using SharedModels;
using SharedModels.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure
{
    public interface IMessagePublisher
    {
        Task PublishOrderCreatedMessage(OrderDTO orderDTO);
    }
}
