using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Data;
using ProductApi.Models;
using SharedModels.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure
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
            _bus.SubscribeAsync<CustomerValidatedMessage>("MessageListener_ProductApi_CustomerValidatedMessage",
                CustomerValidatedAsync, x => x.WithTopic("CustomerValidatedX"));

            lock (this)
            {
                Monitor.Wait(this);
            }
        }

        /// <summary>
        /// Updating the order according to the message received.
        /// </summary>
        /// <param name="cvMessage"></param>
        /// <returns></returns>
        public async Task CustomerValidatedAsync(CustomerValidatedMessage cvMessage)
        {
            using var scope = _provider.CreateScope();
            {
                var services = scope.ServiceProvider;
                var productRepo = services.GetService<IRepository<Product>>();

                List<Product> productsToUpdate = await productRepo.GetAllWhere(x => cvMessage.ProductIDs.Contains(x.ID));
                foreach (Product product in productsToUpdate)
                {
                    int numberToIncrementWith = cvMessage.ProductIDs.Where(x => x == product.ID).Count();
                    product.AmountOfTimesBought += numberToIncrementWith;
                }

                await productRepo.UpdateRange(productsToUpdate);
            }
        }
    }
}
