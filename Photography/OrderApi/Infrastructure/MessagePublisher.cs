using EasyNetQ;
using SharedModels;
using SharedModels.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure
{
    public class MessagePublisher : IMessagePublisher, IDisposable
    {
        private readonly IBus _bus;
        public MessagePublisher(string connectionString)
        {
            _bus = RabbitHutch.CreateBus(connectionString);
        }

        /// <summary>
        /// Once class is disposed, we also dispose the rabbit bus.
        /// </summary>
        public void Dispose()
        {
            _bus.Dispose();
        }

        /// <summary>
        /// Publishing message in order once an order has been created.
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="customerID"></param>
        public async Task PublishOrderCreatedMessage(OrderDTO orderDTO)
        {
            OrderCreatedMessage orderCreatedMsg = new OrderCreatedMessage() { OrderDTO = orderDTO };
            await _bus.PublishAsync(orderCreatedMsg, "CreatedOrder");
        }
    }
}
