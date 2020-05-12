using EasyNetQ;
using SharedModels.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly string _orderCreatedMessageTopic = "created";
        private readonly IBus _bus;
        public MessagePublisher(string connectionString)
        {
            _bus = RabbitHutch.CreateBus(connectionString);
        }

        /// <summary>
        /// Publishing message in order once an order has been created.
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="customerID"></param>
        public void PublishOrderCreatedMessage(int orderID, int customerID)
        {
            OrderCreatedMessage orderCreatedMsg = new OrderCreatedMessage() { CustomerID = customerID, OrderID = orderID };
            _bus.Publish(orderCreatedMsg, _orderCreatedMessageTopic);
        }
    }
}
