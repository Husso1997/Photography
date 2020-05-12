using CustomerApi.Data;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using SharedModels.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerApi.Infrastructure
{
    public class MessageListener
    {
        private readonly IServiceProvider _provider;
        private IBus _bus;
        private readonly string _connectionString;

        public MessageListener(IServiceProvider provider, string connectionString)
        {
            _provider = provider;
            _connectionString = connectionString;
        }

        /// <summary>
        /// This is the method in order to start subscribing, blocking the thread so it keeps running in order to listen.
        /// </summary>
        public void StartSubscription()
        {
            _bus = RabbitHutch.CreateBus(_connectionString);
            _bus.SubscribeAsync<OrderCreatedMessage>("MessageListener_OrderApi_OrderCreatedMessage",
                OrderCreatedAsync, x => x.WithTopic("CreatedOrder"));

            lock (this)
            {
                Monitor.Wait(this);
            }
        }

        public async Task OrderCreatedAsync(OrderCreatedMessage ocMessage)
        {
            using var scope = _provider.CreateScope();
            {
                var services = scope.ServiceProvider;
                var cusRepo = services.GetService<IRepository<Customer>>();
                var msgPublisher = services.GetService<IMessagePublisher>();

                var customerExists = await cusRepo.GetById(ocMessage.OrderDTO.CustomerID) is null ? false : true;
                CustomerValidatedMessage customerValidatedMessage = new CustomerValidatedMessage()
                {
                    OrderID = ocMessage.OrderDTO.ID,
                    CustomerExists = customerExists,
                    ProductIDs = customerExists ? ocMessage.OrderDTO.Products.Select(x => x.ProductID).ToList() : new List<int>()

                };

                await msgPublisher.PublishCustomerValidatedMessage(customerValidatedMessage);

            }
        }
    }
}
