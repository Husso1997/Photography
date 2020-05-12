using EasyNetQ;
using SharedModels;
using SharedModels.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Infrastructure
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

        public async Task PublishCustomerValidatedMessage(CustomerValidatedMessage csMessage)
        {
            string topic = csMessage.CustomerExists ? "CustomerValidatedX" : "CustomerValidated";
            await _bus.PublishAsync(csMessage, topic);
        }
    }
}
