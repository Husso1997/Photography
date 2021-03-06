﻿using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Data;
using OrderApi.Models;
using SharedModels.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure
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
            _bus.SubscribeAsync<CustomerValidatedMessage>("MessageListener_OrderApi_CustomerValidatedMessage",
                CustomerValidatedAsync, x => x.WithTopic("CustomerValidated#"));

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
                var orderRepo = services.GetService<IRepository<Order>>();

                var orderToUpdate = await orderRepo.GetById(cvMessage.OrderID);
                if(!(orderToUpdate is null))
                {
                    orderToUpdate.OrderProgress = cvMessage.CustomerExists ? OrderProgress.Accepted : OrderProgress.Cancelled;
                    await orderRepo.Update(orderToUpdate);
                }
            }
        }
    }
}
